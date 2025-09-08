using UnityEngine;

public class AudioDelayPlayer : MonoBehaviour
{
    public AudioSource audioSource;   // 拖入 AudioManager 上的 AudioSource
    public float delayTime = 3f;      // 幾秒後播放

    void Start()
    {
        // 延遲呼叫 PlayAudio
        Invoke(nameof(PlayAudio), delayTime);
    }

    void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("音效開始播放");
        }
    }
}

