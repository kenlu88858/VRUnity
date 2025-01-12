using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    private bool isRotating = false; // 判斷門是否正在旋轉
    private bool isOpen = false; // 判斷門是否處於打開狀態
    private Quaternion originalRotation; // 初始角度
    private Quaternion targetRotation; // 目標旋轉角度
    private float rotationSpeed = 200f; // 旋轉速度

    void Start()
    {
        // 設定初始角度為當前角度
        originalRotation = transform.rotation;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 平滑旋轉到目標角度
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false; // 停止旋轉
            }
        }
    }

    private void OnMouseDown()
    {
        // 當物件被滑鼠點擊時
        if (!isRotating)
        {
            isRotating = true;
            if (isOpen)
            {
                // 如果門是打開的，目標角度設為原始角度
                targetRotation = originalRotation;
            }
            else
            {
                // 如果門是關閉的，目標角度設為旋轉 120 度
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 90f, transform.eulerAngles.z);
            }
            isOpen = !isOpen; // 切換門的狀態
        }
    }
}
