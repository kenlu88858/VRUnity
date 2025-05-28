using UnityEngine;
using UnityEngine.UI;

public class ShowButtonWhenArrived : MonoBehaviour
{
    public Transform targetObject;      // 被移動的物體
    public Vector3 targetPosition;      // 目標位置
    public float threshold = 0.1f;      // 判定到達的距離容許誤差
    public Button buttonToShow;         // 要顯示的按鈕

    private bool hasShown = false;      // 避免重複執行

    void Start()
    {
        if (buttonToShow != null)
        {
            buttonToShow.gameObject.SetActive(false); // 一開始隱藏按鈕
        }
    }

    void Update()
    {
        if (!hasShown && targetObject != null)
        {
            float distance = Vector3.Distance(targetObject.position, targetPosition);
            if (distance <= threshold)
            {
                buttonToShow.gameObject.SetActive(true); // 顯示按鈕
                hasShown = true;
            }
        }
    }
}
