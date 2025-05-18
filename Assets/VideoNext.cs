using UnityEngine;
using UnityEngine.Video;

public class VideoNext : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject nextButton;
    public AudioClip instructionVoice;

    private AudioSource audioSource;

    void Start()
    {
        nextButton.SetActive(false); // 一開始先關閉按鈕
        videoPlayer.loopPointReached += OnVideoFinished;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        nextButton.SetActive(true); // 顯示按鈕

        if (instructionVoice != null)
        {
            audioSource.PlayOneShot(instructionVoice); // 播放語音
        }
    }
}
