using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class trainingCountdownBarController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillImage;              // 進度條 (Image Type 設為 Filled)
    public TextMeshProUGUI text1;        // 倒數前顯示的文字1
    public TextMeshProUGUI text2;        // 倒數前顯示的文字2
    public TextMeshProUGUI finishText;   // 倒數結束後顯示的文字
    public Button finishButton;          // 倒數結束後顯示的按鈕

    [Header("Countdown Settings")]
    public float countdownDuration = 5f;   // 倒數多久
    public float delayBeforeStart = 2f;    // 點按鈕後延遲多久開始倒數

    [Header("Trigger Button")]
    public Button triggerButton;           // 觸發倒數的按鈕

    private float timer;
    private bool isCounting = false;

    void Start()
    {
        // 一開始隱藏完成UI
        if (finishText != null) finishText.gameObject.SetActive(false);
        if (finishButton != null) finishButton.gameObject.SetActive(false);

        // 綁定觸發按鈕事件
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(OnTriggerButtonClicked);
        }
    }

    private void OnTriggerButtonClicked()
    {
        Debug.Log("觸發按鈕被點擊，倒數將在 " + delayBeforeStart + " 秒後開始");
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

        // 顯示提示文字
        if (text1 != null) text1.gameObject.SetActive(true);
        if (text2 != null) text2.gameObject.SetActive(true);

        // 隱藏完成UI
        if (finishText != null) finishText.gameObject.SetActive(false);
        if (finishButton != null) finishButton.gameObject.SetActive(false);

        // 確保進度條顯示
        if (fillImage != null) fillImage.gameObject.SetActive(true);
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
        // 隱藏倒數中的文字 & 進度條
        if (text1 != null) text1.gameObject.SetActive(false);
        if (text2 != null) text2.gameObject.SetActive(false);
        if (fillImage != null) fillImage.gameObject.SetActive(false);

        // 顯示完成後的文字與按鈕
        if (finishText != null) finishText.gameObject.SetActive(true);
        if (finishButton != null) finishButton.gameObject.SetActive(true);

        Debug.Log("倒數結束，顯示完成UI");
    }
}
