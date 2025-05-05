using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空間
using System.Collections;

public class case3ButtonWhisper : MonoBehaviour
{
    public AudioSource audioSource; // 參考 AudioSource
    public Button playButton; // 參考 UI 按鈕

    public case1whisper_texttospeech whisperScript;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource 未手動設定，嘗試自動尋找！");
            audioSource = GameObject.Find("AudioManager")?.GetComponent<AudioSource>();
        }

        if (playButton == null)
        {
            Debug.LogError("playButton 沒有被設定！");
            return;
        }

        Debug.Log("按鈕成功綁定 PlayVoice 方法！");
        playButton.onClick.AddListener(PlayVoice);
    }

    void PlayVoice()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 沒有找到！");
            return;
        }

        if (audioSource.isPlaying)
            audioSource.Stop(); // 停止當前播放的音頻

        Debug.Log("播放音頻！");
        audioSource.Play(); // 播放音頻
        StartCoroutine(WaitForAudioFinish());
    }

    private IEnumerator WaitForAudioFinish()
    {
        // 等待音頻播放結束
        yield return new WaitForSeconds(audioSource.clip.length);

        // 音頻播放結束後開始錄音辨識
        whisperScript.StartRecording(); // 假設 StartRecording 是 whisper_texttospeech 中的開始錄音方法
    }

}
