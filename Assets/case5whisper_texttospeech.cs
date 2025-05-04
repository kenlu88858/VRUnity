//case5whisper_texttospeech
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class case5whisper_texttospeech : MonoBehaviour
{
    public SafeBoxController safeBoxController;
    private string microphoneDevice;
    private Coroutine recordingCoroutine;
    public string savePath;
    public string Targetsentence; //不要加上標點符號、空格等等
    public string saveFileName = "recordedAudio.wav";  // 音頻保存的檔案名

    public float recordDuration = 10f; // 錄音時間
    public float waitTime = 2f; // 每次辨識後等待時間

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
            microphoneDevice = Microphone.devices[1]; // 使用第二個麥克風設備
            savePath = Path.Combine(Application.persistentDataPath, saveFileName); // 設定保存路徑
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

            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);
            yield return new WaitForSeconds(recordDuration);
            Microphone.End(microphoneDevice);

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

        if (Targetsentence == cleanedText)
        {
            Debug.Log("你說對了!");
            if (audioSource2.isPlaying) audioSource2.Stop();
            audioSource1.Play();
            isTrue = true;

            if (heading4 != null)
            {
                heading4.text = "\n\n語音辨識完成\n\n你說對了";
            }
	    // 顯示按鈕並更改按鈕文字
	    nextbutton.SetActive(true);

	    // 確保按鈕上的文字被正確修改
	    TextMeshProUGUI buttonText = nextbutton.GetComponentInChildren<TextMeshProUGUI>();
	    buttonText.text = "下一步";
	    // 呼叫 SafeBoxController 的 ToggleSafe()
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
                //heading4.text = "請再試一次";
		//heading4.text = "請看著保險箱並和我複誦一次以下文字\n\n<color=#F7FF01>\"帳本就放在保險箱裡\n你可以安心了\"</color>\n\n請複誦";
		heading4.text = falseRecognitionText;

            }
        }

        Debug.Log("語音辨識結果: " + cleanedText);
        Debug.Log(count);
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

        Microphone.End(microphoneDevice);
        isTrue = false;
        Debug.Log("錄音流程已手動停止！");
    }
}
