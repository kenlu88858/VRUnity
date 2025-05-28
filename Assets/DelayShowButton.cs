using UnityEngine;
using UnityEngine.UI;

public class DelayShowButton : MonoBehaviour
{
    public GameObject buttonToShow;  // 👉 第二個按鈕（目標）
    public float delaySeconds = 3f;  // ⏱️ 延遲幾秒

    public void OnFirstButtonClick()
    {
        StartCoroutine(ShowAfterDelay());
    }

    System.Collections.IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(true); // ✅ 顯示第二個按鈕
        }
    }
}

