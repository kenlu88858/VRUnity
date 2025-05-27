using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoNextButtonController : MonoBehaviour
{
    [Header("影片播放器")]
    public VideoPlayer videoPlayer;

    [Header("下一幕按鈕")]
    public GameObject nextSceneButton;

    [Header("語音提示播放器")]
    public AudioSource audioSource; // 用來播放語音提示

    void Start()
    {
        // 一開始先隱藏「下一幕」按鈕
        nextSceneButton.SetActive(false);

        // 影片播完後執行指定方法
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        nextSceneButton.SetActive(true); // 顯示「下幕」按鈕

        // 播放語音提示
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // 播放語音提示
        }
        else
        {
            Debug.LogError("AudioSource 或語音剪輯沒有設定！");
        }
    }
}
