using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color hoverColor = Color.red;
    private Color originalColor;
    private bool isClicked = false;
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

        // 加入 AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked && btnImage != null)
        {
            btnImage.color = hoverColor;

            // 播放 hover 音效
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
            btnImage.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;

        if (btnImage != null)
        {
            btnImage.color = hoverColor;
        }

        // 播放 click 音效
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}