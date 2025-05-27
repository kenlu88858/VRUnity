using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class case7_SelectionManager : MonoBehaviour
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

    private Dictionary<Button, bool> buttonSelections = new Dictionary<Button, bool>();

    void Start()
    {
        buttonsGroup.SetActive(false);
        confirmButton.SetActive(false);

        foreach (var btn in optionButtons)
        {
            buttonSelections[btn] = false;
            btn.onClick.AddListener(() => OnOptionClicked(btn));
        }

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        buttonsGroup.SetActive(true);

        // 播放語音提示
        if (instructionAudio != null)
        {
            instructionAudio.Play();
        }
    }

    void OnOptionClicked(Button btn)
    {
        buttonSelections[btn] = !buttonSelections[btn];
        btn.GetComponent<Image>().color = buttonSelections[btn] ? Color.yellow : Color.white;

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
}
