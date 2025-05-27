using UnityEngine;

public class CameraPositionTracker : MonoBehaviour
{
    void Update()
    {
        Vector3 camPosition = Camera.main.transform.position;
        Debug.Log("主攝影機位置：" + camPosition);
    }
}
