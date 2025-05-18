using UnityEngine;

public class HideAfterDelay : MonoBehaviour
{
    public float delayTime = 15f; // 幾秒後隱藏物件

    private void Start()
    {
        Invoke(nameof(HideObject), delayTime);
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }
}
