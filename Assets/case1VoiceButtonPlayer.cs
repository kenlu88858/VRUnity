using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空間

public class case1VoiceButtonPlayer : MonoBehaviour
{
    public AudioSource audioSource; // 參考 AudioSource
    public AudioSource audioSource1; 
    public Button playButton; // 參考 UI 按鈕

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

        if (audioSource1.isPlaying)
            audioSource1.Stop(); // 停止當前播放的音頻

        Debug.Log("播放音頻！");
        audioSource.Play(); // 播放音頻
    }
}