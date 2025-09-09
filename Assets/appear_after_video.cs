using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ShowCanvasAfterVideo : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;       // 指向播放影片的 VideoPlayer
    public float videoDelay = 2f;         // 幾秒後開始播放影片

    [Header("Canvas Settings")]
    public GameObject canvasToShow;       // 要顯示的 Canvas

    [Header("Audio Settings")]
    public AudioSource audioSource;       // 用來播放語音的 AudioSource
    public AudioClip promptClip;          // 提示語音（影片開始後播）
    public AudioClip finishClip;          // 結束語音（影片結束時播）

    void Start()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false); // 一開始隱藏 Canvas

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false; // 🔴 確保不要自動播放
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊事件
        }

        // 啟動延遲播放影片
        StartCoroutine(StartVideoWithDelay());
    }

    IEnumerator StartVideoWithDelay()
    {
        yield return new WaitForSeconds(videoDelay);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            Debug.Log("影片開始播放");

            // 播放提示語音
            if (audioSource != null && promptClip != null)
            {
                audioSource.PlayOneShot(promptClip);
                Debug.Log("播放提示語音");
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (canvasToShow != null)
        {
            canvasToShow.SetActive(true);  // 影片播放完顯示 Canvas
            Debug.Log("影片結束，顯示 Canvas");
        }

        // 播放結束語音
        if (audioSource != null && finishClip != null)
        {
            audioSource.PlayOneShot(finishClip);
            Debug.Log("播放結束語音");
        }
    }
}
