using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip ClickedSound;
    public AudioClip HoverSound;

    private Button button;
    private AudioSource source;

    void Start()
    {
        // 確保這個物件上有 Button 組件
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on " + gameObject.name);
            return;
        }

        // 新增 AudioSource 並綁定
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 設定為 2D 音效

        // 綁定按鈕點擊事件
        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverSound != null)
        {
            source.PlayOneShot(HoverSound);
        }
    }

    void PlayClickSound()
    {
        if (ClickedSound != null)
        {
            source.PlayOneShot(ClickedSound);
        }
    }
}