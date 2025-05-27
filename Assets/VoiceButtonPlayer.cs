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

        Debug.Log("按鈕成功綁定 PlayVoice 方法！");
        playButton.onClick.AddListener(() => StartCoroutine(PlayVoiceCoroutine()));
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

        // 稍微等待一下，確保 audioSource.isPlaying 會變 true
        yield return new WaitForSeconds(0.1f);

        // 等待語音播放完
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