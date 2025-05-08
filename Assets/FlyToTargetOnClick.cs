using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class FlyToTargetOnClick : MonoBehaviour
{
    public Transform targetPoint; // 目標位置（你面前）
    public XRRayInteractor rayInteractor; // 射線互動器
    public InputActionProperty triggerAction; // trigger 按鍵動作
    public float moveSpeed = 5f;

    private bool shouldMove = false;

    void Update()
    {
        // 按下 trigger 時檢查射線是否點到自己
        if (triggerAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform == this.transform)
                {
                    shouldMove = true; // 開始移動
                }
            }
        }

        if (shouldMove)
        {
            // 線性插值靠近目標位置
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPoint.rotation, 360 * Time.deltaTime);
        }
    }
}
