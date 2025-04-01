using UnityEngine;
using TMPro;

public class DoorLock : MonoBehaviour
{
    public GameObject lockMessage; // 拖入 UI 文字
    public float messageDuration = 2f; // 顯示幾秒

    void Start()
    {
        lockMessage.SetActive(false); // 確保一開始是隱藏的
    }

    public void ShowLockMessage()
    {
        lockMessage.SetActive(true); // 顯示「已上鎖」
        Invoke("HideMessage", messageDuration); // 設定 2 秒後關閉
    }

    void HideMessage()
    {
        lockMessage.SetActive(false);
    }
}

