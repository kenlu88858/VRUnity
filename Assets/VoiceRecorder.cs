using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VoiceRecorder : MonoBehaviour
{
    public int recordDuration = 5; // 錄音秒數
    public AudioSource audioSource;
    private AudioClip recordedClip;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "recorded_audio.wav");
    }

    public void StartAutoRecordAfterPrompt()
    {
        StartCoroutine(PlayPromptThenRecord());
    }

    private IEnumerator PlayPromptThenRecord()
    {
        // 播放語音提示，這裡假設你有設定好 audioSource.clip
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        // 開始錄音
        recordedClip = Microphone.Start(null, false, recordDuration, 44100);
        yield return new WaitForSeconds(recordDuration);

        Microphone.End(null);
        Debug.Log("錄音結束");

        // 存檔成 WAV
        WavUtility.SaveWav(filePath, recordedClip);
        Debug.Log("WAV 儲存成功: " + filePath);

        // 開始辨識
        StartCoroutine(UploadWavToServer(filePath));
    }

    private IEnumerator UploadWavToServer(string path)
    {
        byte[] audioBytes = File.ReadAllBytes(path);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioBytes, "recorded_audio.wav", "audio/wav");

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/transcribe", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("語音辨識失敗: " + www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            Debug.Log("辨識結果: " + result);

            // 根據辨識結果做反應（可客製化）
            if (result.Contains("我們一起看看家裡有哪些東西需要補充,這樣我們才不會買到重複的東西,你看這些蔬菜已經夠了,我們來買些其他的吧!")) // 這裡填你預期的句子
            {
                Debug.Log("你說對了！");
                // 例如：播放成功語音
            }
            else
            {
                Debug.Log("請再試一次。");
                // 例如：播放錯誤提示
            }
        }
    }
    void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("❌ 沒有偵測到麥克風裝置！");
            return;
        }

        Debug.Log("🎤 開始錄音...");
        recordedClip = Microphone.Start(null, false, recordDuration, 44100);

        if (recordedClip == null)
        {
            Debug.LogError("❌ 錄音失敗，AudioClip 為 null");
        }
        else
        {
            Debug.Log($"✅ 錄音成功：長度為 {recordedClip.length} 秒");

            // 指派給 AudioSource 播放以做測試
            audioSource.clip = recordedClip;
            Invoke(nameof(TestPlay), 1f);
        }
    }

    void TestPlay()
    {
        Debug.Log("🔊 播放剛才錄到的聲音...");
        audioSource.Play();
    }

}
