using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SafeBoxController : MonoBehaviour
{
    public Animator animator;  // 連接 Animator
    public bool isOpen = false; // 追蹤開關狀態
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // 自動獲取 Animator
        }

        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // 訂閱 Select Enter（被點擊時觸發）
            grabInteractable.selectEntered.AddListener(OnClicked);
        }
    }

    void OnClicked(SelectEnterEventArgs args)
    {
        ToggleSafe();
    }

    void ToggleSafe()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log("保險箱狀態：" + (isOpen ? "開啟" : "關閉"));
    }
}
