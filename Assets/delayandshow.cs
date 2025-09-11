using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowButtonWithDelay : MonoBehaviour
{
    [Header("Button Settings")]
    public Button targetButton;   // 要顯示的按鈕
    public float delayTime = 3f;  // 延遲幾秒出現

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false); // 一開始隱藏
            StartCoroutine(ShowButtonAfterDelay());
        }
    }

    IEnumerator ShowButtonAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        targetButton.gameObject.SetActive(true); // 延遲後顯示
    }
}

