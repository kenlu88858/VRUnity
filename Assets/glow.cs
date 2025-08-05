using UnityEngine;

public class LightOnObject : MonoBehaviour
{
    public Light lightSource; // 用來指向光源

    void Start()
    {
        // 創建並設置 Point Light
        lightSource = gameObject.AddComponent<Light>();
        lightSource.type = LightType.Point; // 也可以使用 LightType.Spot
        lightSource.range = 1f; // 設定光源範圍
        lightSource.intensity = 1f; // 設定光源強度
        lightSource.color = Color.yellow; // 設定光源顏色
    }
}
