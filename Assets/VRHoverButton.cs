using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color hoverColor = Color.red;        // 🔴 碰到時的紅色
    public Color clickedColor = Color.yellow;   // 🟡 點擊後的黃色（可切換）
    
    private Color originalColor;
    private bool isClicked = false;             // 用來切換點擊狀態
    private Image btnImage;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        btnImage = GetComponent<Image>();
        if (btnImage != null)
        {
            originalColor = btnImage.color;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked && btnImage != null)
        {
            btnImage.color = hoverColor; // 🔴 碰到變紅

            if (hoverSound != null)
            {
                audioSource.PlayOneShot(hoverSound);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked && btnImage != null)
        {
            btnImage.color = originalColor; // 離開還原原色
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = !isClicked; // 每次點擊切換狀態

        if (btnImage != null)
        {
            btnImage.color = isClicked ? clickedColor : originalColor;
        }

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}

