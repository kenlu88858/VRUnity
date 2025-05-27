using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class case4_SelectionManager : MonoBehaviour
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

    private Dictionary<Button, bool> buttonSelections = new Dictionary<Button, bool>();
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();
    private AudioSource hoverAudioSource;

    void Start()
    {
        buttonsGroup.SetActive(false);
        confirmButton.SetActive(false);

        // 播放 hover 音效的 AudioSource（用共用的）
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

            // 加入事件觸發器 for Hover
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
        }
    }

    IEnumerator WaitForInstructionAudio()
    {
        instructionAudio.Play();
        yield return new WaitWhile(() => instructionAudio.isPlaying);
        buttonsGroup.SetActive(true);
    }

    void OnOptionClicked(Button btn)
    {
        buttonSelections[btn] = !buttonSelections[btn];
        btn.GetComponent<Image>().color = buttonSelections[btn] ? Color.yellow : originalColors[btn];

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

        // 指標進入：變紅 + 播音效
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) =>
        {
            Image img = btn.GetComponent<Image>();
            if (img != null)
                img.color = hoverColor;

            if (hoverSound != null && hoverAudioSource != null)
                hoverAudioSource.PlayOneShot(hoverSound);
        });
        trigger.triggers.Add(entryEnter);

        // 指標離開：恢復原色（若沒被選）
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) =>
        {
            if (!buttonSelections[btn]) // 沒被選中才恢復
            {
                Image img = btn.GetComponent<Image>();
                if (img != null)
                    img.color = originalColors[btn];
            }
        });
        trigger.triggers.Add(entryExit);
    }
}
