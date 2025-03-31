using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light myLight;  // 燈光元件
    public float blinkInterval = 0.2f; // 每次閃爍的間隔時間

    void Start()
    {
        if (myLight == null)
            myLight = GetComponent<Light>();

        StartCoroutine(BlinkLight());
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            myLight.enabled = !myLight.enabled;  // 切換燈光開關
            yield return new WaitForSeconds(blinkInterval); // 等待指定時間後再切換
        }
    }
}