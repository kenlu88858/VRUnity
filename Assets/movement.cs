using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils; // XR Origin 用的命名空間

public class XRJoystickMovement : MonoBehaviour
{
    public InputActionProperty moveInput; // 搖桿輸入
    public float moveSpeed = 2.0f; // 移動速度

    private XROrigin xrOrigin; // XR Origin 物件

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>(); // 取得 XR Origin
    }

    void Update()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>(); // 讀取搖桿輸入
        Vector3 move = new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;

        // 移動 XR Origin
        xrOrigin.transform.position += xrOrigin.transform.TransformDirection(move);
    }
}

