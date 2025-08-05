using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private Renderer objectRenderer;
    public Material highlightMaterial; // Highlight 材質

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = highlightMaterial; // 遊戲開始時就套用 Highlight 材質
    }
}
