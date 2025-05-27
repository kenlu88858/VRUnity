using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class case2_ShowButtonAfterAudio : MonoBehaviour
{
    public AudioSource audioSource;  // 指向你的 AudioSource
    public GameObject button;        // 指向你要顯示的按鈕

    void Start()
    {
        // 一開始先隱藏按鈕
        button.SetActive(false);

        // 如果 AudioSource 一開始就播放，直接等它播完
        StartCoroutine(WaitForAudioEnd());
    }

    IEnumerator WaitForAudioEnd()
    {
        // 等到 AudioSource 開始播放
        while (!audioSource.isPlaying)
            yield return null;

        // 等到它播放結束
        while (audioSource.isPlaying)
            yield return null;

        // 顯示按鈕
        button.SetActive(true);
    }
}
