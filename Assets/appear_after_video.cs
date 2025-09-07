using UnityEngine;
using UnityEngine.Video;

public class ShowCanvasAfterVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // 指向播放影片的 VideoPlayer
    public GameObject canvasToShow;   // 要顯示的 Canvas

    void Start()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false); // 一開始隱藏

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊事件
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);  // 影片播放完顯示
    }
}
