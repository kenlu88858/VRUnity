using UnityEngine;
using System.Collections;

public class case2_ShowButtonAfterAudio : MonoBehaviour
{
    [Header("要播放的 AudioSource")]
    public AudioSource audioSource;

    [Header("播完才顯示的按鈕")]
    public GameObject buttonToShow;

    [Header("如果勾選 → 進場自動播放；否則就要手動呼叫 PlayThenShow()")]
    public bool playOnStart = false;

    void Awake()
    {
        // 一開始先把按鈕隱藏
        if (buttonToShow != null)
            buttonToShow.SetActive(false);
    }

    void Start()
    {
        // 如果開了 playOnStart，場景一開始就播
        if (playOnStart)
            PlayThenShow();
    }

    /// <summary>
    /// 公開方法：播放音頻，等播完再顯示按鈕。
    /// Inspector 可以把它加到 OnClick 裡。
    /// </summary>
    public void PlayThenShow()
    {
        if (audioSource == null || buttonToShow == null)
        {
            Debug.LogWarning("請先在 Inspector 指派 audioSource 與 buttonToShow！");
            return;
        }

        // 隱藏按鈕（防重複呼叫）
        buttonToShow.SetActive(false);

        // 播放
        audioSource.Play();

        // 啟協程等結束
        StartCoroutine(WaitAndShow());
    }

    IEnumerator WaitAndShow()
    {
        // 等到真正開始播放
        yield return new WaitUntil(() => audioSource.isPlaying);
        // 等到播完
        yield return new WaitWhile(() => audioSource.isPlaying);
        // 顯示按鈕
        buttonToShow.SetActive(true);
    }
}
