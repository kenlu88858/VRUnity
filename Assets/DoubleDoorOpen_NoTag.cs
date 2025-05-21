using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorOpen_NoTag : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public GameObject triggerTarget; // XR Origin

    private bool isOpened = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 進入：" + other.name);

        // 改成允許 triggerTarget 或其子物件觸發
        if (!isOpened && other.transform.IsChildOf(triggerTarget.transform))
        {
            isOpened = true;
            if (leftDoor != null) leftDoor.gameObject.SetActive(false);
            if (rightDoor != null) rightDoor.gameObject.SetActive(false);
        }
    }
}
