using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class case4_whisper_texttospeech : MonoBehaviour
{
    private string microphoneDevice;
    private Coroutine recordingCoroutine;
    public string savePath;
    public string Targetsentence;
    public string saveFileName = "recordedAudio.wav";

    public AudioSource errorAudioSource;
    public float recordDuration = 10f;
    public float waitTime = 2f;

    private bool isTrue = false;
    public GameObject nextbutton;

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;

    public float whis_FontSize;

    private bool showErrorMessage = false;
    private int retryCount = 0;

    [TextArea]
    public string grab;

    [TextArea]
    public string grab1;

    [TextArea]
    public string recongnize;

    [TextArea]
    public string finish;

    void Start()
    {
        nextbutton.SetActive(false);
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    public void StartRecording()
    {
        if (recordingCoroutine != null)
        {
            Debug.LogWarning("錄音流程已經在執行中，跳過重複啟動。");
            return;
        }
        recordingCoroutine = StartCoroutine(RecordingLoop());
    }

    private IEnumerator RecordingLoop()
    {
        while (!isTrue)
        {
            if (showErrorMessage)
            {
                followtext.text = "複誦內容有誤 請再試一次\n" + grab;
                followtext1.text = "\n" + grab1;
            }
            else
            {
                followtext.text = grab;
                followtext1.text = grab1;
            }

            followtext.fontSize = whis_FontSize;
            followtext1.fontSize = whis_FontSize;

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Debug.Log("請開始說話...");

            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);
            yield return new WaitForSeconds(recordDuration);
            Microphone.End(microphoneDevice);

            followtext.text = recongnize;
            followtext.fontSize = whis_FontSize;
            followtext1.text = "";
            followtext1.fontSize = whis_FontSize;

            Debug.Log("語音錄製完成，開始辨識...");

            audioSource1.Play();
            SaveAudioClipAsWav(recordedClip, savePath);
            Debug.Log("WAV 檔案儲存於: " + savePath);

            yield return StartCoroutine(SendAudioToServer(savePath));
            yield return new WaitForSeconds(waitTime);
        }

        followtext.text = finish;
        followtext.fontSize = whis_FontSize;
        followtext1.text = "";
        followtext1.fontSize = whis_FontSize;

        Debug.Log("停止錄音，語音辨識已結束。");
        recordingCoroutine = null;
        StopRecording();
    }

    public static void SaveAudioClipAsWav(AudioClip clip, string path)
    {
        byte[] audioData = case4_WavUtility2.FromAudioClip(clip);
        File.WriteAllBytes(path, audioData);
    }

    private IEnumerator SendAudioToServer(string audioFilePath)
    {
        string serverUrl = "http://127.0.0.1:5000/transcribe";
        WWWForm form = new WWWForm();
        byte[] audioData = File.ReadAllBytes(audioFilePath);
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");

        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string rawText = www.downloadHandler.text;
            Debug.Log("伺服器回應: " + rawText);

            string extractedText = ExtractTextFromJson(rawText);
            string cleanedText = RemovePunctuationAndWhitespace(extractedText);

            float matchRatio = CalculateMatchRatio(Targetsentence, cleanedText);
            Debug.Log($"比對相似度: {matchRatio * 100:0.0}%");

            if (matchRatio >= 0.5f || retryCount >= 1)
            {
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop();
                }
                Debug.Log("辨識成功！");
                audioSource2.Play();
                isTrue = true;
                showErrorMessage = false;
                nextbutton.SetActive(true);
                retryCount = 0;
            }
            else
            {
                retryCount++;
                if (retryCount >= 2)
                {
                    Debug.Log("已重試一次，自動結束辨識流程。");
                    audioSource2.Play();
                    isTrue = true;
                    showErrorMessage = false;
                    nextbutton.SetActive(true);
                    retryCount = 0;
                }
                else
                {
                    if (errorAudioSource != null)
                    {
                        errorAudioSource.Play();
                        yield return new WaitWhile(() => errorAudioSource.isPlaying);
                        Debug.Log("播放錯誤提示，重試一次！");
                        showErrorMessage = true;
                        audioSource.Play();
                    }
                }
            }

            Debug.Log("語音辨識結果: " + cleanedText);
        }
        else
        {
            Debug.LogError("辨識錯誤: " + www.error);
        }
    }

    private float CalculateMatchRatio(string target, string input)
    {
        int matchCount = 0;
        int len = Mathf.Min(target.Length, input.Length);

        for (int i = 0; i < len; i++)
        {
            if (target[i] == input[i])
            {
                matchCount++;
            }
        }

        return (float)matchCount / target.Length;
    }

    private string ExtractTextFromJson(string jsonText)
    {
        try
        {
            var jsonObj = JsonUtility.FromJson<ResponseData>(jsonText);
            return jsonObj.text;
        }
        catch
        {
            Debug.LogError("無法解析 JSON，請確認伺服器回應格式");
            return jsonText;
        }
    }

    private string RemovePunctuationAndWhitespace(string input)
    {
        return Regex.Replace(input, @"\W+", "");
    }

    [System.Serializable]
    private class ResponseData
    {
        public string text;
    }

    public void StopRecording()
    {
        if (recordingCoroutine != null)
        {
            StopCoroutine(recordingCoroutine);
            recordingCoroutine = null;
        }

        if (Microphone.IsRecording(microphoneDevice))
        {
            Microphone.End(microphoneDevice);
        }
        Debug.Log("錄音流程已手動停止！");
    }
}
