using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class Whisper_texttospeech : MonoBehaviour
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

    [TextArea] public string grab;
    [TextArea] public string grab1;
    [TextArea] public string recongnize;
    [TextArea] public string finish;

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
            Debug.LogError("❌ 無法找到麥克風裝置！");
        }
    }

    public void StartRecording()
    {
        if (recordingCoroutine != null)
        {
            Debug.LogWarning("⚠️ 錄音流程已在執行中，跳過重複啟動！");
            return;
        }

        Debug.Log("🎤 開始錄音流程...");
        recordingCoroutine = StartCoroutine(RecordingLoop());
    }

    private IEnumerator RecordingLoop()
    {
        while (!isTrue)
        {
            followtext.text = showErrorMessage ? "複誦內容有誤 請再試一次\n" + grab : grab;
            followtext1.text = grab1;
            followtext.fontSize = whis_FontSize;
            followtext1.fontSize = whis_FontSize;

            while (audioSource.isPlaying) yield return null;

            Debug.Log("📢 請開始說話...");

            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);
            yield return new WaitForSeconds(recordDuration);
            Microphone.End(microphoneDevice);

            followtext.text = recongnize;
            followtext1.text = "";
            followtext.fontSize = whis_FontSize;
            followtext1.fontSize = whis_FontSize;

            audioSource1.Play();
            SaveAudioClipAsWav(recordedClip, savePath);
            Debug.Log("💾 錄音檔已儲存: " + savePath);

            yield return StartCoroutine(SendAudioToServer(savePath));
            yield return new WaitForSeconds(waitTime);
        }

        followtext.text = finish;
        followtext1.text = "";
        followtext.fontSize = whis_FontSize;
        followtext1.fontSize = whis_FontSize;

        Debug.Log("✅ 錄音結束");
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
        byte[] audioData = File.ReadAllBytes(audioFilePath);

        // Debug 檢查
        Debug.Log("🎯 Targetsentence: " + Targetsentence);
        Debug.Log("📦 音檔大小: " + audioData.Length + " bytes");

        // 防呆檢查
        if (string.IsNullOrEmpty(Targetsentence))
        {
            Debug.LogError("❌ 傳送失敗：Targetsentence 為空！");
            yield break;
        }

        if (audioData == null || audioData.Length == 0)
        {
            Debug.LogError("❌ 傳送失敗：錄音檔為空！");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");
        form.AddField("text", Targetsentence);

        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string rawText = www.downloadHandler.text;
            Debug.Log("🌐 Server response: " + rawText);

            string extractedText = ExtractTextFromJson(rawText);
            string cleanedText = RemovePunctuationAndWhitespace(extractedText);

            float matchRatio = CalculateMatchRatio(Targetsentence, cleanedText);
            Debug.Log($"✅ 比對相似度: {matchRatio * 100:0.0}%");

            if (matchRatio >= 0.5f || retryCount >= 1)
            {
                if (audioSource1.isPlaying) audioSource1.Stop();
                audioSource2.Play();
                isTrue = true;
                showErrorMessage = false;
                nextbutton.SetActive(true);
                retryCount = 0;
            }
            else
            {
                retryCount++;
                showErrorMessage = true;

                if (retryCount >= 2)
                {
                    Debug.Log("⚠️ 已重試 1 次，自動通過！");
                    audioSource2.Play();
                    isTrue = true;
                    nextbutton.SetActive(true);
                    retryCount = 0;
                }
                else
                {
                    Debug.LogWarning("⚠️ 辨識不符，重試中...");
                    if (errorAudioSource != null)
                    {
                        errorAudioSource.Play();
                        yield return new WaitWhile(() => errorAudioSource.isPlaying);
                        audioSource.Play();
                    }
                }
            }

            Debug.Log("🧠 最終辨識內容: " + cleanedText);
        }
        else
        {
            Debug.LogError("❌ 辨識錯誤: " + www.error);
        }
    }

    private float CalculateMatchRatio(string target, string input)
    {
        if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(input)) return 0f;

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
            Debug.LogError("❌ 無法解析 JSON，請確認格式！");
            return jsonText;
        }
    }

    private string RemovePunctuationAndWhitespace(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
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

        Debug.Log("⛔ 錄音流程手動停止");
    }
}
