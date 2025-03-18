using System.Collections;
using UnityEngine;

public class spotlight : MonoBehaviour
{
    private Light lightComponent;  // 燈光組件
    public float onTime = 1f;      // 開啟燈光的時間
    public float offTime = 1f;     // 關閉燈光的時間

    void Start()
    {
        lightComponent = GetComponent<Light>();  // 取得燈光組件
        if (lightComponent != null)
        {
            // 開始 Coroutine
            StartCoroutine(ToggleLight());
        }
    }

    // Coroutine 控制燈光開關
    IEnumerator ToggleLight()
    {
        while (true)
        {
            lightComponent.enabled = true;   // 開啟燈光
            yield return new WaitForSeconds(onTime); // 等待 onTime 秒
            lightComponent.enabled = false;  // 關閉燈光
            yield return new WaitForSeconds(offTime); // 等待 offTime 秒
        }
    }
}
