using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class case4_whisper_texttospeech : MonoBehaviour
{
    private string microphoneDevice;
    private Coroutine recordingCoroutine; // 放在類別最上方
    public string savePath;
    public string Targetsentence; //不要加上標點符號、空格等等
    public string saveFileName = "recordedAudio.wav";  // 音頻保存的檔案名

    public float recordDuration = 10f; // 錄音時間
    public float waitTime = 2f; // 每次辨識後等待時間

    private bool isTrue = false;
    public GameObject nextbutton;

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;

    public float whis_FontSize ;

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
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0]; // 使用第一個麥克風設備
            savePath = Path.Combine(Application.persistentDataPath, saveFileName); // 設定保存路徑
            //StartCoroutine(RecordingLoop()); // 啟動循環錄音
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
        while (!isTrue) // 無限循環錄音
        {
            followtext.text = grab;
            followtext.fontSize = whis_FontSize;
            followtext1.text = grab1;
            followtext1.fontSize = whis_FontSize;
            while (audioSource.isPlaying)
            {
                yield return null;  // 等待直到音頻播放結束
            }
            
            Debug.Log("請開始說話...");
            
            // 開始錄音
            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);

            // 等待 10 秒
            yield return new WaitForSeconds(recordDuration);

            // 停止錄音
            Microphone.End(microphoneDevice);
            followtext.text = recongnize;
            followtext.fontSize = whis_FontSize;
            followtext1.text = "";
            followtext1.fontSize = whis_FontSize;
            Debug.Log("語音錄製完成，開始辨識...");

            // 保存音頻檔案
            audioSource1.Play();
            SaveAudioClipAsWav(recordedClip, savePath);
            Debug.Log("WAV 檔案儲存於: " + savePath);

            // 發送音頻到伺服器辨識
            yield return StartCoroutine(SendAudioToServer(savePath));

            // 等待 2 秒再繼續下一次錄音
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

    // 將錄製的音頻保存為 WAV 檔案
    public static void SaveAudioClipAsWav(AudioClip clip, string path)
    {
        byte[] audioData = case4_WavUtility2.FromAudioClip(clip); // 轉換 AudioClip 為 WAV 格式的 byte[]
        File.WriteAllBytes(path, audioData); // 儲存檔案
    }

    // 發送音頻檔案到伺服器
    private IEnumerator SendAudioToServer(string audioFilePath)
    {
        string serverUrl = "http://127.0.0.1:5000/transcribe";  // 伺服器的 URL
        WWWForm form = new WWWForm();
        byte[] audioData = File.ReadAllBytes(audioFilePath);  // 讀取音頻檔案

        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");  // 假設檔案是 WAV 格式

        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string rawText = www.downloadHandler.text;
            Debug.Log("伺服器回應: " + rawText);

            string extractedText = ExtractTextFromJson(rawText);

            string cleanedText = RemovePunctuationAndWhitespace(extractedText);
            if(Targetsentence == cleanedText){
                if(audioSource1.isPlaying){
                    audioSource1.Stop();
                }
                Debug.Log("你說對了!");
                audioSource2.Play();
                isTrue = true;
            }
            else{
                Debug.Log("播放音頻！");
                audioSource.Play();
                //yield return new WaitForSeconds(audioSource.clip.length);
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
            return jsonObj.text; // 假設伺服器回傳格式為 {"text": "你好，這是測試！"}
        }
        catch
        {
            Debug.LogError("無法解析 JSON，請確認伺服器回應格式");
            return jsonText; // 如果解析失敗，直接回傳原始字串
        }
    }

    private string RemovePunctuationAndWhitespace(string input)
    {
        return Regex.Replace(input, @"\W+", ""); // \W+ 表示移除所有非字母數字的字符
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
            Microphone.End(microphoneDevice); // 避免在沒錄音時叫 End 出錯
        }
        Debug.Log("錄音流程已手動停止！");
    }
}
