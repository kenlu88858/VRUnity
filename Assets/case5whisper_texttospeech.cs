using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class case5whisper_texttospeech : MonoBehaviour
{
    public SafeBoxController safeBoxController;
    public CountdownBarController countdownBar; // 新增：對倒數條控制器的引用
    private string microphoneDevice;
    private Coroutine recordingCoroutine;
    public string savePath;
    public string Targetsentence;
    public string saveFileName = "recordedAudio.wav";

    public float recordDuration = 10f;
    public float waitTime = 2f;

    public bool isTrue = false;
    public GameObject nextbutton;

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;
    public TextMeshProUGUI heading4;
    public float whis_FontSize;
    public int count = 0;

    [TextArea]
    public string falseRecognitionText;

    [TextArea]
    public string grab;

    [TextArea]
    public string grab2;

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
            followtext.text = grab;
            followtext.fontSize = whis_FontSize;
            followtext1.text = grab2;
            followtext1.fontSize = whis_FontSize;

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Debug.Log("請開始說話...");

            // 新增：在開始錄音前，啟動倒數條
            if (countdownBar != null)
            {
                countdownBar.StartCountdown(recordDuration);
            }

            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);
            yield return new WaitForSeconds(recordDuration);
            Microphone.End(microphoneDevice);

            // 新增：在錄音結束後，停止倒數條
            if (countdownBar != null)
            {
                countdownBar.StopCountdown();
            }

            followtext.text = recongnize;
            followtext.fontSize = whis_FontSize;
            followtext1.text = "";
            audioSource2.Play();

            Debug.Log("語音錄製完成，開始辨識...");
            SaveAudioClipAsWav(recordedClip, savePath);
            Debug.Log("WAV 檔案儲存於: " + savePath);

            yield return StartCoroutine(SendAudioToServer(savePath));

            yield return new WaitForSeconds(waitTime);
        }

        followtext.text = finish;
        followtext.fontSize = whis_FontSize;
        followtext1.text = "";
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
        if (heading4 != null)
        {
            heading4.text = "\n\n語音辨識中\n\n請稍候";
        }

        count += 1;
        string serverUrl = "http://localhost:5000/transcribe";
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
            string cleanedTarget = RemovePunctuationAndWhitespace(Targetsentence);

            float matchRatio = CalculateMatchRatio(cleanedTarget, cleanedText);
            Debug.Log($"比對相似度: {matchRatio * 100:0.0}%");

            bool pass = false;

            if (matchRatio >= 0.5f || count >= 2)
            {
                pass = true;
            }

            if (pass)
            {
                Debug.Log("你說對了!");
                if (audioSource2.isPlaying) audioSource2.Stop();
                audioSource1.Play();
                isTrue = true;

                if (heading4 != null)
                {
                    heading4.text = "\n\n語音辨識完成\n\n你說對了";
                }

                nextbutton.SetActive(true);
                TextMeshProUGUI buttonText = nextbutton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "下一步";

                if (safeBoxController != null)
                {
                    safeBoxController.ToggleSafe();
                }
            }
            else
            {
                Debug.Log("播放音頻！");
                if (audioSource2.isPlaying) audioSource2.Stop();
                audioSource.Play();

                if (heading4 != null)
                {
                    heading4.text = falseRecognitionText;
                }
            }

            Debug.Log("語音辨識結果: " + cleanedText);
            Debug.Log("count: " + count);
        }
        else
        {
            Debug.LogError("辨識錯誤: " + www.error);

            if (heading4 != null)
            {
                heading4.text = "辨識失敗，請稍後再試";
            }
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

    private float CalculateMatchRatio(string target, string input)
    {
        if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(input)) return 0f;

        int matchCount = 0;
        int minLength = Mathf.Min(target.Length, input.Length);

        for (int i = 0; i < minLength; i++)
        {
            if (target[i] == input[i]) matchCount++;
        }

        return (float)matchCount / target.Length;
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

        // 新增：在手動停止時也確保倒數條被隱藏
        if (countdownBar != null)
        {
            countdownBar.StopCountdown();
        }

        isTrue = false;
        Debug.Log("錄音流程已手動停止！");
    }
}
