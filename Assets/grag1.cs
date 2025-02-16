using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;  // 用來儲存物體和滑鼠之間的偏移
    private float zDistance; // 用來儲存物體的 z 軸位置（保持不變）

    void OnMouseDown()
    {
        // 當按下滑鼠時，計算物體和滑鼠之間的偏移
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        // 當拖曳物體時，更新物體位置
        transform.position = GetMouseWorldPosition() + offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 取得滑鼠在世界座標中的位置
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistance; // 保持物體的 z 軸深度
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
