using System.Collections;
using UnityEngine;

public class CabbageClickMover : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 3f;

    private bool shouldMove = false;

    public void OnClicked()
    {
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                shouldMove = false;

                // ✅ 標記已完成
                TaskProgressManager.Instance.cabbageMoved = true;
                Debug.Log("✅ 高麗菜移動完成");
            }
        }
    }
}
