using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class HighlightVegetable : MonoBehaviour
{
    public Material highlightMaterial;  // 發光材質
    private Material originalMaterial;  // 原本的材質
    private Renderer objectRenderer;

    public InputActionProperty triggerAction; // 來自 XR Controller 的按鈕輸入

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    private void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
        {
            // 當玩家按下按鈕時，高麗菜變亮
            objectRenderer.material = highlightMaterial;
        }
        else if (triggerAction.action.WasReleasedThisFrame())
        {
            // 放開按鈕後，高麗菜回復原狀
            objectRenderer.material = originalMaterial;
        }
    }
}

