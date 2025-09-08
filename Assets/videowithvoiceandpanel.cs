using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoWithVoiceAndPanel : MonoBehaviour
{
    public VideoPlayer videoPlayer;         // 🎬 影片播放器
    public AudioSource tipAudioSource;      // 🔊 播放提示語音的 AudioSource
    public GameObject popupPanel;           // 🪧 包含按鈕與提示資訊的整塊面板

    private bool firstPlay = true;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // 影片結束時執行
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        // 第第一次需要延迟 6 秒再播放影片
        if (firstPlay)
        {
            firstPlay = false;
            StartCoroutine(PlayVideoAfterDelay(7f)); // 延迟6秒再播
        }
        else
        {
            videoPlayer.Play();
        }
    }

    private IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(PlayVoiceAndShowPanel()); // 影片結束後處理
    }

    private IEnumerator PlayVoiceAndShowPanel()
    {
        yield return new WaitForSeconds(0.5f);
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (tipAudioSource != null)
        {
            tipAudioSource.Play();
        }
    }

    public void ReplayVideo()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }
}
