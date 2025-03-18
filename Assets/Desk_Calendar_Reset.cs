using System.Collections;
using UnityEngine;

public class Desk_Calendar_Reset : MonoBehaviour
{
    private Vector3 originalPosition;  // 記錄原始位置
    private Quaternion originalRotation; // 記錄原始旋轉
    private Rigidbody rb; // 物件的 Rigidbody

    private void Start()
    {
        // 在遊戲開始時，記錄物品的原始位置和旋轉
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 確保物件有 Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("ItemReset 需要 Rigidbody，請在物品上添加 Rigidbody！");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 當物品碰到 "Ground" 物件時，開始倒數 1 秒後重置
        if (collision.gameObject.CompareTag("ground"))
        {
            StartCoroutine(ResetAfterDelay(1f)); // 延遲 1 秒
        }
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定秒數

        // 重置物品位置與旋轉
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // 清除 Rigidbody 的速度與旋轉，避免物品還有動能
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}