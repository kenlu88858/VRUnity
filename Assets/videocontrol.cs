using UnityEngine;
using UnityEngine.Video;

public class PlayVideoAfterCanvasHidden : MonoBehaviour
{
    public GameObject canvasObject;   // 你想監控的 Canvas
    public VideoPlayer videoPlayer;   // 影片播放器

    private bool hasStarted = false;
    private bool wasActive = true;

    void Update()
    {
        if (hasStarted) return;
        if (canvasObject == null) return;

        // 監測 Canvas 是否從 active 變成 inactive
        if (wasActive && !canvasObject.activeSelf)
        {
            hasStarted = true;

            if (videoPlayer == null)
            {
                Debug.LogWarning("未指定 VideoPlayer！");
                return;
            }

            // 確保 videoPlayer 本身是 active
            if (!videoPlayer.gameObject.activeInHierarchy)
            {
                videoPlayer.gameObject.SetActive(true);
            }

            videoPlayer.Play();
            Debug.Log("Canvas 不見後開始播放影片！");
        }

        wasActive = canvasObject.activeSelf;
    }
}
