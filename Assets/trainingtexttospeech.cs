using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class trainingCountdownBarController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillImage;             // 前景進度條
    public GameObject backgroundBar;    // 背景
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI finishText;
    public Button finishButton;

    [Header("Countdown Settings")]
    public float countdownDuration = 10f;   // 倒數多久
    public float delayBeforeStart = 5f;     // 點按鈕後延遲多久開始倒數

    [Header("Trigger Button")]
    public Button triggerButton;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip startClip;
    public AudioClip finishClip;

    private float timer;
    private bool isCounting = false;

    // --- Start 函式已修改 ---
    void Start()
    {
        // 只隱藏進度條與背景
        if (fillImage != null) fillImage.gameObject.SetActive(false);
        if (backgroundBar != null) backgroundBar.SetActive(false);

        // 其他UI元素將保持其在Editor中的初始狀態

        // 綁定觸發按鈕事件
        if (triggerButton != null)
            triggerButton.onClick.AddListener(OnTriggerButtonClicked);
    }

    private void OnTriggerButtonClicked()
    {
        Debug.Log($"觸發按鈕被點擊，倒數將在 {delayBeforeStart} 秒後開始");
        StartCoroutine(StartCountdownWithDelay());
    }

    IEnumerator StartCountdownWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        StartCountdown(countdownDuration);
    }

    public void StartCountdown(float seconds)
    {
        timer = seconds;
        fillImage.fillAmount = 1f;
        isCounting = true;

        // 顯示背景與進度條
        if (backgroundBar != null) backgroundBar.SetActive(true);
        if (fillImage != null) fillImage.gameObject.SetActive(true);

        // 顯示提示文字
        if (text1 != null) text1.gameObject.SetActive(true);
        if (text2 != null) text2.gameObject.SetActive(true);

        // 隱藏完成UI
        if (finishText != null) finishText.gameObject.SetActive(false);
        if (finishButton != null) finishButton.gameObject.SetActive(false);

        // 播放開始語音
        if (audioSource != null && startClip != null)
            audioSource.PlayOneShot(startClip);
    }

    void Update()
    {
        if (isCounting)
        {
            timer -= Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / countdownDuration);

            if (timer <= 0f)
            {
                isCounting = false;
                OnCountdownFinished();
            }
        }
    }

    private void OnCountdownFinished()
    {
        // 隱藏倒數中文字
        if (text1 != null) text1.gameObject.SetActive(false);
        if (text2 != null) text2.gameObject.SetActive(false);

        // 顯示完成後的文字與按鈕
        if (finishText != null) finishText.gameObject.SetActive(true);
        if (finishButton != null) finishButton.gameObject.SetActive(true);

        // 播放結束語音
        if (audioSource != null && finishClip != null)
            audioSource.PlayOneShot(finishClip);

        Debug.Log("倒數結束，顯示完成UI + 播放語音");

        // **隱藏進度條與背景**
        if (fillImage != null) fillImage.gameObject.SetActive(false);
        if (backgroundBar != null) backgroundBar.SetActive(false);
    }
}