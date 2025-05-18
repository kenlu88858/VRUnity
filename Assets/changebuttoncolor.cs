using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Button myButton;
    public Color selectedColor = Color.green; // 點擊後的顏色

    private Color originalColor;

    void Start()
    {
        // 儲存原始顏色
        originalColor = myButton.image.color;

        // 綁定點擊事件
        myButton.onClick.AddListener(ChangeButtonColor);
    }

    void ChangeButtonColor()
    {
        myButton.image.color = selectedColor;
    }

    // 若你想恢復原本顏色，也可以加：
    public void ResetColor()
    {
        myButton.image.color = originalColor;
    }
}
