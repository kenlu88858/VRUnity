/*using UnityEngine;

public class PassbookController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera cam;
    public Transform safeBoxSlot; // 存摺放進去的指定位置
    private bool isPlaced = false; // 存摺是否已放入

    void Start()
    {
        cam = Camera.main; // 主攝影機
    }

    void OnMouseDown()
    {
        if (!isPlaced) // 只有還沒放進去時才能拖動
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            // 確保存摺只會在保險箱開啟時移動
            SafeBoxController safeBox = FindObjectOfType<SafeBoxController>();
            if (safeBox != null && safeBox.isOpen)
            {
                float distance = Vector3.Distance(transform.position, safeBoxSlot.position);
                if (distance < 1f) // 距離小於 1f 才算放進去
                {
                    transform.position = safeBoxSlot.position;
                    isPlaced = true; // 標記為已放入
                    Debug.Log("存摺放入保險箱！");
                }
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePoint);
    }
}  */