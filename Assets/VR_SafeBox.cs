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

    public void ToggleSafe()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log("保險箱狀態：" + (isOpen ? "開啟" : "關閉"));
    }
}

/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SafeBoxController : MonoBehaviour
{
    public Animator animator;                          // Animator 連結到保險箱上
    public XRBaseInteractable buttonInteractable;      // 拖進 Inspector 的 XR Button 物件
    private bool isClosed = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (buttonInteractable != null)
        {
            buttonInteractable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (!isClosed)
        {
            animator.SetBool("isOpen", false);
            isClosed = true;
            Debug.Log("保險箱已關閉");
        }
    }
}*/
