using UnityEngine;
using UnityEngine.Video;

public class VideoButtonController_3 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject[] optionButtons;       // 前6個按鈕
    public GameObject confirmButton;         // 第7個按鈕
    public AudioClip instructionVoice;       // 提示語音
    public GameObject instructionCanvas;     // 語音提示 Canvas（一開始出現）
    public GameObject optionPromptCanvas;    // 顯示按鈕時出現的提示文字 Canvas ← ⬅️ 新增這個

    private AudioSource audioSource;
    private bool hasSelectedOption = false;

    void Start()
    {
        // 關閉所有按鈕
        foreach (GameObject btn in optionButtons)
            btn.SetActive(false);

        confirmButton.SetActive(false);

        // 關閉兩個提示Canvas
        if (instructionCanvas != null)
            instructionCanvas.SetActive(false);

        if (optionPromptCanvas != null)
            optionPromptCanvas.SetActive(false); // ⬅️ 新增這行

        // 註冊 callback
        videoPlayer.loopPointReached += OnVideoFinished;

        // 音效播放器
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (instructionCanvas != null)
            instructionCanvas.SetActive(true);

        if (instructionVoice != null)
        {
            audioSource.PlayOneShot(instructionVoice);
            Invoke(nameof(ShowOptionButtons), instructionVoice.length);
        }
        else
        {
            ShowOptionButtons();
        }
    }

    void ShowOptionButtons()
    {
        // 關閉語音提示 Canvas
        // if (instructionCanvas != null)
        //     instructionCanvas.SetActive(false);

        // 顯示選項提示文字的 Canvas
        if (optionPromptCanvas != null)
            optionPromptCanvas.SetActive(true); // ⬅️ 新增這行

        // 顯示前6個選項按鈕
        foreach (GameObject btn in optionButtons)
            btn.SetActive(true);
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