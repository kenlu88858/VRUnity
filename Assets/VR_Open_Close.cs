using UnityEngine;

public class SafeBoxController : MonoBehaviour
{
    public Animator animator;  // 連接 Animator
    public bool isOpen = false; // 追蹤開關狀態

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // 自動獲取 Animator
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 滑鼠左鍵點擊
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // 點擊到的是保險箱
                {
                    ToggleSafe();
                }
            }
        }
    }

    void ToggleSafe()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log("保險箱狀態：" + (isOpen ? "開啟" : "關閉"));
    }
}
