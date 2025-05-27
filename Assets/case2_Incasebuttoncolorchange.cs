using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class case2_Incasebuttoncolorchange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button button;
    private Color originalColor;
    private AudioSource audioSource;
    public AudioClip hoverSound;  // 指到時的音效
    //public AudioClip clickSound;  // 點擊時的音效

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        originalColor = button.image.color;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = Color.red;
        Debug.Log($"{gameObject.name} ➤ 滑入");
        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
            Debug.Log("按鈕音效");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = originalColor;
        Debug.Log($"{gameObject.name} ➤ 滑出");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
