using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;

public class case1whisper_texttospeech : MonoBehaviour
{
    private string microphoneDevice;
    private Coroutine recordingCoroutine;
    public string savePath;
    public string Targetsentence;
    public string Targetsentence1;
    public string saveFileName = "recordedAudio.wav";

    public float recordDuration = 10f;
    public float waitTime = 2f;

    public bool isTrue = false;
    public GameObject nextbutton;

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;
    public float whis_FontSize;
    int count = 0;

    [TextArea] public string grab;
    [TextArea] public string grab2;
    [TextArea] public string recongnize;
    [TextArea] public string finish;

    // --- (新增) 引用你的倒數條控制器 ---
    [Header("UI 控制器")]
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
            Debug.LogError("偵測不到麥克風設備!");
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
            while (audioSource.isPlaying || audioSource3.isPlaying)
            {
                yield return null;
            }

            Debug.Log("請開始說話...");

            // --- (修改) 在開始錄音的同時，啟動倒數條 ---
            if (countdownBar != null)
            {
                countdownBar.StartCountdown(recordDuration);
            }

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
        count++;
        if (count >= 2)
        {
            if (audioSource2.isPlaying) audioSource2.Stop();
            Debug.Log("你說對了!");

            followtext.text = finish;
            followtext.fontSize = whis_FontSize;
            followtext1.text = "";
            followtext1.fontSize = whis_FontSize;

            audioSource1.Play();
            while (audioSource1.isPlaying) yield return null;

            nextbutton.SetActive(true);
            isTrue = true;
        }
        else
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
                float score = ParseScoreFromJson(rawText);
                Debug.Log("相似度分數: " + score);

                if (score >= 0.8f)
                {
                    if (audioSource2.isPlaying) audioSource2.Stop();
                    Debug.Log("你說對了!");

                    followtext.text = finish;
                    followtext.fontSize = whis_FontSize;
                    followtext1.text = "";
                    audioSource1.Play();
                    while (audioSource1.isPlaying) yield return null;

                    nextbutton.SetActive(true);
                    isTrue = true;
                }
                else
                {
                    followtext.text = grab;
                    followtext.fontSize = whis_FontSize;
                    followtext1.text = grab2;
                    followtext1.fontSize = whis_FontSize;
                    if (audioSource2.isPlaying) audioSource2.Stop();
                    Debug.Log("播放音頻！");
                    audioSource3.Play();
                }
                Debug.Log("嘗試次數: " + count);
            }
            else
            {
                Debug.LogError("辨識錯誤: " + www.error);
            }
        }
    }

    private float ParseScoreFromJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return -1f;
        int start = json.IndexOf('[') + 1;
        int end = json.IndexOf(']');
        if (start > 0 && end > start)
        {
            string number = json.Substring(start, end - start);
            if (float.TryParse(number, out float score))
            {
                return score;
            }
        }
        return -1f;
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

        // --- (新增) 確保手動停止時，倒數條也會被隱藏 ---
        if (countdownBar != null)
        {
            countdownBar.StopCountdown();
        }

        isTrue = false;
        Debug.Log("錄音流程已手動停止！");
    }
}
