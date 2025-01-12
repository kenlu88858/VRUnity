using UnityEngine;

public class DragAndDestroy : MonoBehaviour
{
    public GameObject Bin; // 指定 Trash can 物件
    public GameObject effectPrefab; // 特效的預製體
    private bool isDragging = false; // 判斷是否正在拖曳
    private Vector3 offset; // 滑鼠與物件中心的偏移量

    void Update()
    {
        if (isDragging)
        {
            // 更新物件位置，使其跟隨滑鼠
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition + offset;
        }
    }

    void OnMouseDown()
    {
        // 開始拖曳，計算滑鼠與物件中心的偏移量
        isDragging = true;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        offset = transform.position - worldPosition;
    }

    void OnMouseUp()
    {
        // 結束拖曳
        isDragging = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // 碰撞到 Trash can 時觸發特效並刪除物件
        if (other.gameObject == Bin)
        {
            // 創建特效
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            // 刪除物件
            Destroy(gameObject);
        }
    }
}
