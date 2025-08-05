using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class case5_SelectionManager : MonoBehaviour
{
    [Header("影片播放器")]
    public VideoPlayer videoPlayer;

    [Header("按鈕群組父物件")]
    public GameObject buttonsGroup;

    [Header("選項按鈕們")]
    public List<Button> optionButtons;

    [Header("確認按鈕")]
    public GameObject confirmButton;

    [Header("語音提示")]
    public AudioSource instructionAudio;

    [Header("Hover 效果")]
    public Color hoverColor = Color.red;
    public AudioClip hoverSound;

    [Header("音效播完後要顯示的 Canvas")]
    public GameObject resultCanvas; // ✅ 新增欄位

    private Dictionary<Button, bool> buttonSelections = new Dictionary<Button, bool>();
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();
    private AudioSource hoverAudioSource;

    void Start()
    {
        buttonsGroup.SetActive(false);
        confirmButton.SetActive(false);

        // ✅ 預設隱藏 resultCanvas
        if (resultCanvas != null)
            resultCanvas.SetActive(false);

        // 共用的 AudioSource 用來播放 hover/click 音效
        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.playOnAwake = false;

        foreach (var btn in optionButtons)
        {
            buttonSelections[btn] = false;

            // 儲存原色
            Image img = btn.GetComponent<Image>();
            if (img != null)
                originalColors[btn] = img.color;

            // 點擊事件
            btn.onClick.AddListener(() => OnOptionClicked(btn));

            // 加入滑過/離開事件
            AddHoverEvents(btn);
        }

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (instructionAudio != null)
        {
            StartCoroutine(WaitForInstructionAudio());
        }
        else
        {
            buttonsGroup.SetActive(true);
            if (resultCanvas != null)
                resultCanvas.SetActive(true); // 若沒語音也顯示 Canvas
        }
    }

    IEnumerator WaitForInstructionAudio()
    {
        instructionAudio.Play();
        yield return new WaitWhile(() => instructionAudio.isPlaying);

        buttonsGroup.SetActive(true);

        // ✅ 語音播完後顯示 Canvas
        if (resultCanvas != null)
            resultCanvas.SetActive(true);
    }

    void OnOptionClicked(Button btn)
    {
        buttonSelections[btn] = !buttonSelections[btn];

        // ✅ 若選中變黃，否則還原
        btn.GetComponent<Image>().color = buttonSelections[btn] ? Color.yellow : originalColors[btn];

        // ✅ 播放點擊音效
        if (hoverSound != null && hoverAudioSource != null)
        {
            hoverAudioSource.PlayOneShot(hoverSound);
        }

        // ✅ 顯示確認按鈕（若至少一個選中）
        bool anySelected = false;
        foreach (var selected in buttonSelections.Values)
        {
            if (selected)
            {
                anySelected = true;
                break;
            }
        }

        confirmButton.SetActive(anySelected);
    }

    void AddHoverEvents(Button btn)
    {
        EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = btn.gameObject.AddComponent<EventTrigger>();
        }

        // ✅ 滑過：只有未被選中才變紅
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) =>
        {
            if (!buttonSelections[btn])
            {
                Image img = btn.GetComponent<Image>();
                if (img != null)
                    img.color = hoverColor;
            }

            if (hoverSound != null && hoverAudioSource != null)
            {
                hoverAudioSource.PlayOneShot(hoverSound);
            }
        });
        trigger.triggers.Add(entryEnter);

        // ✅ 離開：若未選中，恢復原色
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) =>
        {
            if (!buttonSelections[btn])
            {
                Image img = btn.GetComponent<Image>();
                if (img != null)
                    img.color = originalColors[btn];
            }
        });
        trigger.triggers.Add(entryExit);
    }
}
