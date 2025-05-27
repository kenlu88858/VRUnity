using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class case4_VRHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor = Color.red;        // 🔴 指標進入時的紅色
    private Color originalColor;
    private Image btnImage;

    public AudioClip hoverSound;
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
        if (btnImage != null)
        {
            btnImage.color = hoverColor; // 🔴 變紅
        }

        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound); // 播放音效
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnImage != null)
        {
            btnImage.color = originalColor; // 還原原色
        }
    }
}
