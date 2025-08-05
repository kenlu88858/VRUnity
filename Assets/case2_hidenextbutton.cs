using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class case2_hidenextbutton : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 拖曳你的 VideoPlayer 進來
    public GameObject nextButton;  // 拖曳你的右邊按鈕進來

    void Start()
    {
        // 一開始按鈕先隱藏
        nextButton.SetActive(false);

        // 設定影片播放完畢後要執行的函數
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 顯示右邊的按鈕
        nextButton.SetActive(true);
    }
}
