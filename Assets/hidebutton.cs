using UnityEngine;
using UnityEngine.UI;

public class ShowButtonAfterDelay : MonoBehaviour
{
    public Button targetButton;     // 要顯示的按鈕
    public float delayTime = 3f;    // 延遲幾秒後出現

    private void Start()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false); // 一開始先隱藏
            Invoke(nameof(ShowButton), delayTime);    // 延遲執行
        }
    }

    private void ShowButton()
    {
        targetButton.gameObject.SetActive(true); // 顯示按鈕
    }
}
