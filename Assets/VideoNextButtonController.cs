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
    }
}

