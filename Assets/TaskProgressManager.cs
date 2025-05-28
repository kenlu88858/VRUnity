using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProgressManager : MonoBehaviour
{
    public static TaskProgressManager Instance;

    public bool cabbageMoved = false;
    public bool itemFlownToTarget = false;

    public GameObject nextButton;

    void Awake()
    {
        Instance = this;
        if (nextButton != null)
            nextButton.SetActive(false);
    }

    void Update()
    {
        if (cabbageMoved && itemFlownToTarget)
        {
            if (nextButton != null && !nextButton.activeSelf)
            {
                nextButton.SetActive(true);
            }
        }
    }
}
