using UnityEngine;
using UnityEngine.UI;

public class HideCanvas : MonoBehaviour
{
    public GameObject canvas;
    public Button button;
    public float delayTime = 3f;

    private void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
            Invoke(nameof(ShowCanvas), delayTime);
        }

        if (button != null)
        {
            button.onClick.AddListener(HideCanvasAndChildren);
        }
    }

    private void ShowCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    private void HideCanvasAndChildren()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }
}
