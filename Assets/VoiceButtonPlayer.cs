using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VoiceButtonPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Button playButton;
    public Whisper_texttospeech whisperManager;

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

        // 先隱藏按鈕或禁用互動
        playButton.gameObject.SetActive(false);
        // 或：playButton.interactable = false;

        // 開始播放語音與控制顯示按鈕
        StartCoroutine(PlayVoiceCoroutine());
    }

   public IEnumerator PlayVoiceCoroutine()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 沒有找到！");
            yield break;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("播放語音提示！");
        audioSource.Play();

        yield return new WaitForSeconds(0.1f);

        yield return new WaitWhile(() => audioSource.isPlaying);

        Debug.Log("語音提示播放完畢");

        if (whisperManager != null)
        {
            Debug.Log("啟動語音辨識！");
            whisperManager.StartRecording();
        }
        else
        {
            Debug.LogError("whisperManager 尚未綁定！");
        }

        // 播放完後顯示按鈕
        playButton.gameObject.SetActive(true);
        // 或：playButton.interactable = true;

        // 綁定按鈕點擊事件
        Debug.Log("按鈕成功綁定 PlayVoice 方法！");
        playButton.onClick.AddListener(() => StartCoroutine(PlayVoiceCoroutine()));
    }

    void StopVoice()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("語音被停止！");
            audioSource.Stop();
        }
    }
}
