using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceSequenceManager : MonoBehaviour
{
    public List<VoiceButtonPlayer7> voicePlayers;

    void Start()
    {
        StartCoroutine(PlayVoicesInSequence());
    }

    IEnumerator PlayVoicesInSequence()
    {
        foreach (var player in voicePlayers)
        {
            yield return player.PlayVoiceCoroutine(); // 等前一個播放完成
        }

        Debug.Log("所有語音提示播放完成！");
    }
}
