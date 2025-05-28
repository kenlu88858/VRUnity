using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DelayedButtonActivator : MonoBehaviour
{
    public Button buttonA;        // 要延遲顯示的按鈕 A
    public Button buttonB;        // 觸發事件的按鈕 B
    public float delayTime = 3f;  // 延遲秒數

    private void Start()
    {
        // 一開始先隱藏 A
        if (buttonA != null)
            buttonA.gameObject.SetActive(false);

        // 綁定 B 的點擊事件
        if (buttonB != null)
            buttonB.onClick.AddListener(StartDelayToShowA);
    }

    void StartDelayToShowA()
    {
        StartCoroutine(DelayToShowA());
    }

    IEnumerator DelayToShowA()
    {
        yield return new WaitForSeconds(delayTime);

        if (buttonA != null)
            buttonA.gameObject.SetActive(true);
    }
}


