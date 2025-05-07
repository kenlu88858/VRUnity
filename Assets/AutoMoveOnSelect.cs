using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class MoveWhenTriggered : MonoBehaviour
{
    public Transform targetTransform; // 要移動到的目標（冰箱下層）
    public XRRayInteractor rayInteractor; // 射線互動器
    public InputActionReference triggerAction; // XR Controller 的 Select（Trigger）動作

    private bool hasMoved = false;

    void Update()
    {
        if (hasMoved) return;

        if (triggerAction != null && triggerAction.action.triggered)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    MoveToTarget();
                    hasMoved = true;
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }
}
