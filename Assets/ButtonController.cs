using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonController : MonoBehaviour
{
    public Light lamp;        // 連接 Spot Light 或 Point Light
    public AudioSource sound; // 連接 AudioSource 用來播放開關燈的音效
    private bool isOn = false;

    private void Start()
    {
        if (lamp != null)
        {
            lamp.enabled = isOn; // 預設燈是關的
        }
    }

    public void ToggleLamp()
    {
        isOn = !isOn; // 切換狀態
        if (lamp != null)
        {
            lamp.enabled = isOn; // 開關燈
        }

        // 播放開關燈的音效
        if (sound != null)
        {
            sound.Play(); // 播放音效
        }
    }
}


