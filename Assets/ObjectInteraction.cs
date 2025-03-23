using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject backImageUI; // 右下角的背影圖像 UI

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) // 確保是玩家頭部進入
        {
            gameObject.SetActive(false); // 隱藏老人物件
            backImageUI.SetActive(true); // 顯示右下角的背影圖片
        }
    }
    void Update()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        transform.LookAt(Camera.main.transform); // 確保 UI 朝向玩家
    }
}

