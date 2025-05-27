using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class case4_ShowButtonAfterAudio : MonoBehaviour
{
    public AudioSource audioSource;     // 可以是 AudioManager 或本身的 AudioSource
    public GameObject buttonObject;
    public bool autoPlayClip = false;   // 若是自帶 clip，是否要自動播放

    private bool waitingStarted = false;

    void Start()
    {
        if (buttonObject != null)
            buttonObject.SetActive(false);

        // 若指定播放 clip，且 audioSource 有 clip
        if (autoPlayClip && audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // 當音檔正在播放，且還沒開始等待
        if (audioSource != null && audioSource.isPlaying && !waitingStarted)
        {
            StartCoroutine(WaitUntilFinished());
            waitingStarted = true;
        }
    }

    IEnumerator WaitUntilFinished()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        if (buttonObject != null)
            buttonObject.SetActive(true);
    }
}