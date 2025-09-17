using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTriggerTest : MonoBehaviour
{
    // 直接在 Inspector 指派 AudioSource（也可以事先掛在物件上）
    public AudioSource audioSource;

    void Start()
    {
        // 如果 Inspector 沒指定，就自動加一個
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 基本設定
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 設為 2D
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 進入：" + other.name);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("播放語音");
        }
    }
}

