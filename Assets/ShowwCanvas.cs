using UnityEngine;
using UnityEngine.UI;

public class ShowwCanvas : MonoBehaviour
{
    public GameObject canvas;   // 要顯示或隱藏的 Canvas
    public Button button;       // 觸發顯示的按鈕

    private void Start()
    {
        // 一開始就隱藏 Canvas
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        // 按下按鈕時才顯示 Canvas
        if (button != null)
        {
            button.onClick.AddListener(ShowCanvas);
        }
    }

    private void ShowCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }
}
