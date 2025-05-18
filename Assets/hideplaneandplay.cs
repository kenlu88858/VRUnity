using UnityEngine;

public class HideAndPlaySound : MonoBehaviour
{
    public float delayTime = 3f;               // 幾秒後執行
    public AudioSource audioSource;            // 播放音效的 AudioSource
    public AudioClip clipToPlay;               // 指定要播放的音效（可選）

    private void Start()
    {
        Invoke(nameof(HideAndPlay), delayTime);
    }

    private void HideAndPlay()
    {
        // 隱藏 Plane
        gameObject.SetActive(false);

        // 播放音效（如果有指定）
        if (audioSource != null)
        {
            if (clipToPlay != null)
            {
                audioSource.clip = clipToPlay;
            }
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource 沒有設定，無法播放音效！");
        }
    }
}
