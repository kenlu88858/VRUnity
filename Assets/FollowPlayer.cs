using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerCamera; // 設定為 XR Origin 內的 Main Camera
    public Vector3 offset = new Vector3(0.3f, -1.2f, 0.5f); // 右下角的位置偏移

    void Update()
    {
        if (playerCamera != null)
        {
            // 讓老人物件跟隨玩家頭部位置
            transform.position = playerCamera.position + playerCamera.TransformDirection(offset);
            // 讓老人物件面朝玩家的前方
            transform.rotation = Quaternion.LookRotation(playerCamera.forward);
        }
    }
}

