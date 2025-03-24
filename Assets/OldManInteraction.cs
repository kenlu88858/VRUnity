using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OldManInteraction : MonoBehaviour
{
    public GameObject oldManModel; // 原本的老人模型
    public GameObject smallOldManModel; // 右下角的 3D 老人模型
    public Transform playerCamera; // 玩家頭部的 Camera

    private Vector3 offset = new Vector3(0.25f, -1.15f, 0.5f); // 右下角的位置偏移

    void Start()
    {
        smallOldManModel.SetActive(false); // 一開始隱藏右下角 3D 老人
    }

    public void OnInteract()
    {
        oldManModel.SetActive(false); // 隱藏原本的老人
        smallOldManModel.SetActive(true); // 顯示右下角的 3D 老人

        // **方法 1**：將小老人設定為玩家相機的子物件
        smallOldManModel.transform.SetParent(playerCamera, false);

        // 設定相對位置和旋轉
        smallOldManModel.transform.localPosition = offset;
        smallOldManModel.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        // **Update 可以移除，因為 SetParent() 自動處理位置和旋轉**
    }
}

