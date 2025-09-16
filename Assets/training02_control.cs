using UnityEngine;
using UnityEngine.UI;

public class ShowTwoCanvasWithClips : MonoBehaviour
{
    [Header("UI References")]
    public GameObject canvasA;          // 第 1 個要顯示的 Canvas
    public GameObject canvasB;          // 第 2 個要顯示的 Canvas
    public Button triggerButton;        // 觸發顯示的按鈕

    [Header("Audio Clips")]
    public AudioClip newClip;           // 按下按鈕後要播放的語音

    private AudioSource internalSource;

    void Start()
    {
        // 一開始隱藏兩個 Canvas
        if (canvasA) canvasA.SetActive(false);
        if (canvasB) canvasB.SetActive(false);

        // 建立專用 AudioSource
        internalSource = gameObject.AddComponent<AudioSource>();
        internalSource.playOnAwake = false;

        if (triggerButton)
            triggerButton.onClick.AddListener(PlayNewAudioAndShowCanvas);
    }

    private void PlayNewAudioAndShowCanvas()
    {
        // 🔑 停止場景裡所有 AudioSource
        foreach (AudioSource src in FindObjectsOfType<AudioSource>())
        {
            if (src.isPlaying)
                src.Stop();
        }

        // 播放新的語音
        if (newClip)
        {
            internalSource.clip = newClip;
            internalSource.Play();
        }

        // 顯示兩個 Canvas
        if (canvasA) canvasA.SetActive(true);
        if (canvasB) canvasB.SetActive(true);
    }
}