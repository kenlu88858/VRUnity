using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRHoverButton2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color hoverColor = Color.red;
    public Color selectedColor = Color.yellow;

    private Color originalColor;
    private bool isSelected = false;
    private Image btnImage;
    private AudioSource audioSource;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Button button;
    private int buttonIndex;

    void Start()
    {
        btnImage = GetComponent<Image>();
        button = GetComponent<Button>();

        if (btnImage != null)
            originalColor = btnImage.color;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 在 MultipleChoiceManager1 中找自己是第幾顆按鈕
        var manager = FindObjectOfType<MultipleChoiceManager2>();
        if (manager != null)
        {
            for (int i = 0; i < manager.optionButtons.Length; i++)
            {
                if (manager.optionButtons[i] == button)
                {
                    buttonIndex = i;
                    break;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && btnImage != null)
        {
            btnImage.color = hoverColor;
            if (hoverSound != null)
                audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && btnImage != null)
        {
            btnImage.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;

        if (btnImage != null)
        {
            btnImage.color = isSelected ? selectedColor : originalColor;
        }

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // 更新 MultipleChoiceManager1 中的選取狀態
        MultipleChoiceManager1.selectedOptions[buttonIndex] = isSelected;

        // 通知確認按鈕狀態更新
        var manager = FindObjectOfType<MultipleChoiceManager2>();
        if (manager != null)
        {
            manager.UpdateConfirmButton();
        }
    }
}