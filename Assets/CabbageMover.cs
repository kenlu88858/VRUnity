using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageMover : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 2f;

    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void MoveToTarget()
    {
        isMoving = true;
    }
}
