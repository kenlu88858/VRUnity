using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProgressManager : MonoBehaviour
{
    public static TaskProgressManager Instance;

    public bool cabbageMoved = false;
    public bool itemFlownToTarget = false;

    public GameObject nextButton;

    void Awake()
    {
        Debug.Log("✅ TaskProgressManager 已經啟動");
        Instance = this;
        if (nextButton != null)
            nextButton.SetActive(false);
    }

    void Update()
{
    Debug.Log($"cabbageMoved: {cabbageMoved}, itemFlownToTarget: {itemFlownToTarget}");

    if (cabbageMoved && itemFlownToTarget)
    {
        if (!nextButton.activeSelf)
        {
            nextButton.SetActive(true);
            Debug.Log("✅ 顯示下一幕按鈕");
        }
    }
}
}

