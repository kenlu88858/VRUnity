using UnityEngine;
using UnityEngine.Video;

public class VideoButtonController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject[] optionButtons;      // 前6個按鈕
    public GameObject confirmButton;        // 第7個按鈕
    public AudioClip instructionVoice;      // 提示語音

    private AudioSource audioSource;
    private bool hasSelectedOption = false;

    void Start()
    {
        // 先關閉所有按鈕
        foreach (GameObject btn in optionButtons)
            btn.SetActive(false);

        confirmButton.SetActive(false);

        // 註冊 callback
        videoPlayer.loopPointReached += OnVideoFinished;

        // 音效播放器
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 顯示前6個選項按鈕
        foreach (GameObject btn in optionButtons)
            btn.SetActive(true);

        // 播放提示語音
        if (instructionVoice != null)
        {
            audioSource.PlayOneShot(instructionVoice);
        }
    }

    public void OnOptionSelected()
    {
        if (!hasSelectedOption)
        {
            confirmButton.SetActive(true);
            hasSelectedOption = true;
        }
    }
}