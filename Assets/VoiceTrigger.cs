using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    public AudioClip voiceClip;         
    private AudioSource audioSource;    

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = voiceClip;
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // 用 XR Origin 名稱判斷（你可以根據 Hierarchy 裡的名稱微調）
        if (other.gameObject.name.Contains("Camera Offset"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}

