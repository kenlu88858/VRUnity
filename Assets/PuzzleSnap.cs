using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PuzzleSnap : MonoBehaviour
{
    public Transform targetTransform; // 目標位置（在 Inspector 設定）

    public Transform InitTransform; // 起始位置（在 Inspector 設定）
    public float snapDistance ; // 觸發吸附的距離
    public bool smoothSnap = true; // 是否使用平滑移動
    public float snapSpeed = 10f; // 平滑吸附速度
    public float rotationSpeed = 5f;
    public TextMeshProUGUI followtext;
    public float grabbedFontSize;
    
    public GameObject button;
    private Rigidbody rb;
    private bool isSnapped = false;

    private Vector3 snapPosition;
    private Quaternion snapRotation;
    private Vector3 InitPosition;

    private Quaternion InitRotation;

    private XRGrabInteractable grabInteractable;

    [TextArea]
    public string grab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        snapPosition = targetTransform.position; // 確保使用世界座標
        snapRotation = targetTransform.rotation; // 目標旋轉
        InitPosition = InitTransform.position;
        InitRotation = InitTransform.rotation;
        //Debug.Log(snapPosition);
        //Debug.Log(transform.position);
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        //if (isSnapped) return; // 如果已經吸附，不再檢查

        float distance = Vector3.Distance(transform.position, snapPosition);
        Debug.Log("當前距離：" + distance);
        //Debug.Log("拼圖當前座標：" + transform.position + "，目標座標：" + snapPosition);

        if (distance <= snapDistance)
        {
            SnapToTarget();
            button.SetActive(true);
            followtext.text = grab;
            followtext.fontSize = grabbedFontSize;
        }

        if (grabInteractable != null && !grabInteractable.isSelected && distance > snapDistance)
        {
            MoveObjectBack();
        }
    }
    
    void MoveObjectBack(){
        transform.position = Vector3.MoveTowards(transform.position, InitPosition, snapSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, InitRotation, rotationSpeed * Time.deltaTime);
    }

    void SnapToTarget()
    {
        //isSnapped = true;

        ForceRelease();

        if (smoothSnap)
        {
            StartCoroutine(SmoothMoveToTarget());
        }
        else
        {
            transform.position = snapPosition;
            transform.rotation = snapRotation; // 直接設置旋轉
            rb.isKinematic = true;
        }
    }

    IEnumerator SmoothMoveToTarget()
    {
        while (Vector3.Distance(transform.position, snapPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, snapPosition, snapSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, snapRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = snapPosition;
        transform.rotation = snapRotation;
        rb.isKinematic = true;
    }

    void ForceRelease()
    {
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            // 解除 VR/XR 交互物件的抓取
            grabInteractable.interactionManager.SelectExit(grabInteractable.firstInteractorSelecting, grabInteractable);
        }
        
        // 如果沒使用 XR，可以直接取消 Rigidbody 的影響
        rb.isKinematic = true; // 讓物體不受物理影響
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
