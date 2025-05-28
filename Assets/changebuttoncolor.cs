using UnityEngine;
using UnityEngine.UI;

public class ButtonColorToggle : MonoBehaviour
{
    public Button myButton;
    public Color selectedColor = Color.green; // 點擊後的顏色

    private Color originalColor;
    private bool isSelected = false; // 狀態記錄：目前是否為選取色

    void Start()
    {
        originalColor = myButton.image.color;
        myButton.onClick.AddListener(ToggleButtonColor);
    }

    void ToggleButtonColor()
    {
        isSelected = !isSelected; // 切換狀態

        if (isSelected)
        {
            myButton.image.color = selectedColor;
        }
        else
        {
            myButton.image.color = originalColor;
        }
    }

    // 若要重設狀態（可選）
    public void ResetColor()
    {
        isSelected = false;
        myButton.image.color = originalColor;
    }
}
