using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class whisper_texttospeech : MonoBehaviour
{
    private string microphoneDevice;
    private Coroutine recordingCoroutine;
    public string savePath;
    public string Targetsentence;
    public string saveFileName = "recordedAudio.wav";

    public float recordDuration = 10f;
    public float waitTime = 2f;

    private bool isTrue = false;
    private int failCount = 0; // 新增失敗次數計數器
    public int maxFailCount = 1; // 允許的最大失敗次數

    public GameObject nextbutton;
    public TextMeshProUGUI errorTipText;

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource wrongAudioSource;

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;

    public float whis_FontSize;

    [TextArea] public string grab;
    [TextArea] public string grab1;
    [TextArea] public string recongnize;
    [TextArea] public string finish;

    public CountdownBarController countdownBar;

    void Start()
    {
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
            followtext.text = grab;
            followtext.fontSize = whis_FontSize;
            followtext1.text = grab1;
            followtext1.fontSize = whis_FontSize;

            while (audioSource.isPlaying)
                yield return null;

            Debug.Log("請開始說話...");

            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);

            if (countdownBar != null)
            {
                countdownBar.StartCountdown(recordDuration);
            }

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
        nextbutton.SetActive(true);
        Debug.Log("停止錄音，語音辨識已結束。");
        recordingCoroutine = null;
        StopRecording();
    }

    public static void SaveAudioClipAsWav(AudioClip clip, string path)
    {
        byte[] audioData = WavUtility2.FromAudioClip(clip);
        File.WriteAllBytes(path, audioData);
    }

    private IEnumerator SendAudioToServer(string audioFilePath)
    {
        string serverUrl = "http://127.0.0.1:5000/transcribe";
        WWWForm form = new WWWForm();
        byte[] audioData = File.ReadAllBytes(audioFilePath);
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");
        form.AddField("text", Targetsentence);
        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string rawText = www.downloadHandler.text;
            Debug.Log("伺服器回應: " + rawText);

            string extractedText = ExtractTextFromJson(rawText);
            string cleanedText = RemovePunctuationAndWhitespace(extractedText);

            if (Targetsentence == cleanedText || failCount > maxFailCount)
            {
                if (audioSource1.isPlaying)
                    audioSource1.Stop();

                Debug.Log("你說對了!");
                audioSource2.Play();
                isTrue = true;
                errorTipText.gameObject.SetActive(false);
                nextbutton.SetActive(true);
            }
            else
            {
                Debug.Log("播放音頻！");
                wrongAudioSource.Play();
                errorTipText.gameObject.SetActive(true);
                followtext.text = grab;
                followtext.fontSize = whis_FontSize;
                followtext1.text = grab1;
                followtext1.fontSize = whis_FontSize;
                failCount++; // 累加錯誤次數
                yield return new WaitWhile(() => wrongAudioSource.isPlaying);
            }

            Debug.Log("語音辨識結果: " + cleanedText);
        }
        else
        {
            Debug.LogError("辨識錯誤: " + www.error);
        }
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

        if (countdownBar != null)
        {
            countdownBar.StopCountdown();
        }

        Debug.Log("錄音流程已手動停止！");
    }
}