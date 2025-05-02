using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzleSnap : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(30.52f, 38.37f, 30.55f);  // 物品要回到的目標位置
    private Quaternion targetRotation = Quaternion.Euler(0, 90, 180);  // 物品回到目標位置時的目標旋轉（根據需要修改）
    //public Transform targetTransform; // 目標位置（在 Inspector 設定）

    //public Transform InitTransform; // 起始位置（在 Inspector 設定）
    //public float snapDistance ; // 觸發吸附的距離
    public bool smoothSnap = true; // 是否使用平滑移動
    //public float snapSpeed = 10f; // 平滑吸附速度
    //public float rotationSpeed = 5f;
    public float moveSpeed = 5f;      // 物品回到指定位置的速度
    public float rotationSpeed = 50000;  // 旋轉速度，控制物品旋轉的平滑度

    public TextMeshProUGUI followtext;
    public float grabbedFontSize;
    public AudioSource audioSource;
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
        /*rb = GetComponent<Rigidbody>();
        snapPosition = targetTransform.position; // 確保使用世界座標
        snapRotation = targetTransform.rotation; // 目標旋轉
        InitPosition = InitTransform.position;
        InitRotation = InitTransform.rotation;
        //Debug.Log(snapPosition);
        //Debug.Log(transform.position);
        grabInteractable = GetComponent<XRGrabInteractable>();*/
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (grabInteractable != null)
        {
            // 註冊「抓取事件」
            grabInteractable.onSelectEntered.AddListener(OnGrab);
            //grabInteractable.onSelectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        //if (isSnapped) return; // 如果已經吸附，不再檢查

        float distance = Vector3.Distance(transform.position, snapPosition);
        Debug.Log("當前距離：" + distance);
        //Debug.Log("拼圖當前座標：" + transform.position + "，目標座標：" + snapPosition);

        /*if (distance <= snapDistance && !isSnapped)
        {
            SnapToTarget();
            PlaySnapSound();
            button.SetActive(true);
            followtext.text = grab;
            followtext.fontSize = grabbedFontSize;
            isSnapped = true;
        }

        if (grabInteractable != null && !grabInteractable.isSelected && distance > snapDistance)
        {
            MoveObjectBack();
        }*/
    }

    void PlaySnapSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("播放吸附音效！");
        }
        else
        {
            Debug.LogWarning("AudioSource 或 AudioClip 沒有設定！");
        }
    }
    
    /*void MoveObjectBack(){
        transform.position = Vector3.MoveTowards(transform.position, InitPosition, snapSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, InitRotation, rotationSpeed * Time.deltaTime);
    }*/
    private void OnGrab(XRBaseInteractor interactor)
    {
        grabInteractable.enabled = false;
        StartCoroutine(SmoothMoveToTarget());
        PlaySnapSound();
    }
    
    /*void SnapToTarget()
    {
        //isSnapped = true;

        //ForceRelease();

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
    }*/

    IEnumerator SmoothMoveToTarget()
    {
        /*while (Vector3.Distance(transform.position, snapPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, snapPosition, snapSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, snapRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = snapPosition;
        transform.rotation = snapRotation;
        rb.isKinematic = true;*/
        while (Vector3.Distance(transform.position, targetPosition) > 0.0f){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    /*void ForceRelease()
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
    }*/
}
