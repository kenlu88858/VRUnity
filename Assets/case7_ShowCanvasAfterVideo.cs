using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class case7_ShowCanvasAfterVideo : MonoBehaviour
{
    [Header("影片播放器")]
    public VideoPlayer videoPlayer;

    [Header("要顯示的 Canvas 群")]
    public GameObject[] canvasGroup;

    void Start()
    {
        // 一開始隱藏所有 Canvas
        foreach (var canvas in canvasGroup)
        {
            canvas.SetActive(false);
        }

        // 當影片播完觸發事件
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        foreach (var canvas in canvasGroup)
        {
            canvas.SetActive(true);
        }
    }
}
