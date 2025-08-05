using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisplayUI : MonoBehaviour
{
    public GameObject canvasToShow;  // 指向你的 Canvas
    public GameObject planeToShow;   // 指向你的 Plane

    private void Start()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false); // 遊戲開始時隱藏

        if (planeToShow != null)
            planeToShow.SetActive(false); // 同上
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);

        if (planeToShow != null)
            planeToShow.SetActive(true);
    }
}

