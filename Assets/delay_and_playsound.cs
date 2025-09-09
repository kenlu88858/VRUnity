using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit; // 如果是 XR 點擊事件才需要

public class ConditionalDelayedAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clip1;           // 延遲播放的語音1
    public AudioClip clip2;           // 被打斷後播放的語音2
    public float delaySeconds = 3f;   // clip1 延遲時間

    [Header("Trigger Object")]
    public XRBaseInteractable triggerObject; // 被點擊的物體（如果不是 XR，可以用 Button 事件）

    private Coroutine audioCoroutine;
    private bool hasSwitched = false;

    void Start()
    {
        if (audioSource != null && clip1 != null)
        {
            audioCoroutine = StartCoroutine(PlayAudioWithDelay());
        }

        // 綁定 XR 點擊事件（假設 triggerObject 是 XR Grab/Interactable）
        if (triggerObject != null)
        {
            triggerObject.selectEntered.AddListener(OnObjectClicked);
        }
    }

    private IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(delaySeconds);

        if (!hasSwitched) // 只有在還沒被打斷時才播放 clip1
        {
            audioSource.clip = clip1;
            audioSource.Play();
            Debug.Log("延遲 " + delaySeconds + " 秒後播放 clip1");
        }
    }

    private void OnObjectClicked(SelectEnterEventArgs args)
    {
        Debug.Log("物體被點擊，切換語音");

        hasSwitched = true; // 標記已切換

        // 如果正在播放 clip1，中斷它
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // 播放 clip2
        if (clip2 != null)
        {
            audioSource.clip = clip2;
            audioSource.Play();
            Debug.Log("播放 clip2");
        }

        // 停止 clip1 的 coroutine，避免它又啟動
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        if (triggerObject != null)
        {
            triggerObject.selectEntered.RemoveListener(OnObjectClicked);
        }
    }
}


