using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTriggerTest : MonoBehaviour
{
    public AudioClip voiceClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = voiceClip;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 設為2D音效，避免空間定位影響聽不到
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

