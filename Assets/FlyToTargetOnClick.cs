using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class FlyToTargetOnClick : MonoBehaviour
{
    public Transform targetPoint;
    public XRRayInteractor rayInteractor;
    public InputActionProperty triggerAction;
    public float moveSpeed = 5f;

    private bool shouldMove = false;
    private bool hasReachedTarget = false;

    void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform == this.transform)
                {
                    shouldMove = true;
                }
            }
        }

        if (shouldMove && !hasReachedTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPoint.rotation, 360 * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
            {
                hasReachedTarget = true;
                shouldMove = false;

                // ✅ 標記已完成
                TaskProgressManager.Instance.itemFlownToTarget = true;
                Debug.Log("✅ 購物清單移動完成");
            }
        }
    }
}
