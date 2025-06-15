using UnityEngine;
using UnityEngine.Video;

public class VideoButtonController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject[] optionButtons;      // 前6個按鈕
    public GameObject confirmButton;        // 第7個按鈕
    public AudioClip instructionVoice;      // 提示語音

    public GameObject text1;                // 額外的 Text 1
    public GameObject text2;                // 額外的 Text 2
    public GameObject plane1;               // 額外的 Plane 1
    public GameObject plane2;               // 額外的 Plane 2

    private AudioSource audioSource;
    private bool hasSelectedOption = false;

    void Start()
    {
        // 先關閉所有按鈕
        foreach (GameObject btn in optionButtons)
            btn.SetActive(false);

        confirmButton.SetActive(false);

        // 關閉額外的文字與平面
        if (text1 != null) text1.SetActive(false);
        if (text2 != null) text2.SetActive(false);
        if (plane1 != null) plane1.SetActive(false);
        if (plane2 != null) plane2.SetActive(false);

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

        // 顯示額外的物件
        if (text1 != null) text1.SetActive(true);
        if (text2 != null) text2.SetActive(true);
        if (plane1 != null) plane1.SetActive(true);
        if (plane2 != null) plane2.SetActive(true);

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