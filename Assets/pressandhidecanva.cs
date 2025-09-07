using UnityEngine;
using UnityEngine.UI;

public class HideCanvasOnClick : MonoBehaviour
{
    public Canvas targetCanvas;   // 要隱藏的 Canvas
    public Button triggerButton;  // 按下的按鈕

    void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(HideCanvas);
        }
    }

    void HideCanvas()
    {
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false); // 隱藏 Canvas
        }
    }
}
