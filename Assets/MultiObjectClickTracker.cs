using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiObjectClickTracker : MonoBehaviour
{
    public List<GameObject> requiredObjects; // 拖 5 個物件進來
    public GameObject nextButton;

    private HashSet<GameObject> clickedObjects = new HashSet<GameObject>();

    void Start()
    {
        if (nextButton != null)
            nextButton.SetActive(false);
    }

    public void OnObjectTriggered(GameObject obj)
    {
        if (requiredObjects.Contains(obj))
        {
            clickedObjects.Add(obj);
        }

        if (clickedObjects.Count == requiredObjects.Count)
        {
            nextButton.SetActive(true);
        }
    }
}
