using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiobutton2 : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject buttonToShow;

    void OnEnable()
    {
        buttonToShow.SetActive(false);

        if (audioSource.clip == null)
        {
            Debug.LogError("AudioClip 沒有設！");
            return;
        }

        audioSource.Play();
        StartCoroutine(WaitForAudioToEnd());
    }


    IEnumerator WaitForAudioToEnd()
    {
        yield return new WaitUntil(() => audioSource.isPlaying); // 確保真的開始播放
        yield return new WaitWhile(() => audioSource.isPlaying); // 等它播放完

        Debug.Log("語音播放結束，顯示按鈕");
        buttonToShow.SetActive(true);
    }
}
