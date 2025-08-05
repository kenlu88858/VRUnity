using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 offset;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        // 計算滑鼠點擊位置與物體中心的偏移量
        offset = transform.position - GetMouseWorldPos();
        rb.useGravity = false;  // 暫時關閉重力，讓拖動更流暢
	Debug.Log("點擊成功！");  // 測試是否有偵測到滑鼠點擊
    }

    void OnMouseDrag()
    {
        // 持續更新物體位置
        rb.MovePosition(GetMouseWorldPos() + offset);
    }

    void OnMouseUp()
    {
        rb.useGravity = true;  // 拖動結束後重新啟用重力
    }

    private Vector3 GetMouseWorldPos()
    {
        // 取得滑鼠在世界座標的位置
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // 設定 Z 軸距離
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
