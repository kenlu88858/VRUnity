using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public AudioSource audio1; // 語音1
    public AudioSource audio2; // 語音2
    public float delaySeconds = 3f; // 延遲幾秒播放語音1
    public Button interruptButton; // 觸發切換的按鈕

    private Coroutine playCoroutine;

    void Start()
    {
        // 註冊按鈕事件
        interruptButton.onClick.AddListener(OnButtonClicked);

        // 啟動延遲播放語音1
        playCoroutine = StartCoroutine(PlayAudio1WithDelay());
    }

    private IEnumerator PlayAudio1WithDelay()
    {
        // 延遲指定秒數
        yield return new WaitForSeconds(delaySeconds);

        // 播放語音1
        audio1.Play();
    }

    private void OnButtonClicked()
    {
        // 如果語音1還沒開始或正在播放，要中斷
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        if (audio1.isPlaying)
        {
            audio1.Stop();
        }

        // 播放語音2
        audio2.Play();
    }
}

