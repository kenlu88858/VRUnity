using UnityEngine;

public class ShowButtonAfterVoice : MonoBehaviour
{
    public GameObject buttonToShow;     // 👈 要顯示的按鈕
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 一開始先隱藏按鈕
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(false);
        }

        // 播語音並等待播放完
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            StartCoroutine(ShowButtonAfterAudio(audioSource.clip.length));
        }
    }

    System.Collections.IEnumerator ShowButtonAfterAudio(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (buttonToShow != null)
        {
            buttonToShow.SetActive(true); // ✅ 播完才出現
        }
    }
}

