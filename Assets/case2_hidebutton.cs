using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;

public class case2_hidebutton : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 拖曳你的 VideoPlayer 進來
    public GameObject rightButton1;  // 拖曳你的右邊按鈕進來
    public GameObject rightButton2;
    public GameObject rightButton3;
    public GameObject rightButton4;
    public GameObject rightButton5;
    public GameObject rightButton6;
    public GameObject confirmButton;
    public AudioSource audioSource1;
    //public AudioSource audioSource;
    public GameObject hint_plane;
    public GameObject hint_select;

    void Start()
    {
        hint_plane.SetActive(false);
        hint_select.SetActive(false);
        // 一開始按鈕先隱藏
        rightButton1.SetActive(false);
        rightButton2.SetActive(false);
        rightButton3.SetActive(false);
        rightButton4.SetActive(false);
        rightButton5.SetActive(false);
        rightButton6.SetActive(false);
        confirmButton.SetActive(false);

        // 設定影片播放完畢後要執行的函數
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {

        // 顯示右邊的按鈕
        hint_plane.SetActive(true);
        Debug.Log("plane activate");
        audioSource1.Play();
        StartCoroutine(WaitForAudioFinish());


        //confirmButton.SetActive(true);
    }

    private IEnumerator WaitForAudioFinish()
    {
        // 等待直到音訊播放完畢
        while (audioSource1.isPlaying)
        {
            yield return null; // 每幀檢查
        }

        hint_select.SetActive(true);
        //audioSource.Play();
        rightButton1.SetActive(true);
        rightButton2.SetActive(true);
        rightButton3.SetActive(true);
        rightButton4.SetActive(true);
        rightButton5.SetActive(true);
        rightButton6.SetActive(true);
    }
}
