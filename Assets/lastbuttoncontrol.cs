using UnityEngine;
using UnityEngine.UI;

public class VoiceAndButtonController : MonoBehaviour
{
    public AudioSource audioSource;     // 🎤 播放語音的 AudioSource
    public GameObject buttonB;          // 🎯 要在語音播完後出現的按鈕物件

    void Start()
    {
        if (buttonB != null)
        {
            buttonB.SetActive(false);  // 一開始隱藏 ButtonB
        }
    }

    public void PlayVoiceAndShowButton()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            StartCoroutine(WaitAndShowButton(audioSource.clip.length));
        }
    }

    private System.Collections.IEnumerator WaitAndShowButton(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // 等語音播完
        if (buttonB != null)
        {
            buttonB.SetActive(true);  // 顯示 ButtonB
        }
    }
}
