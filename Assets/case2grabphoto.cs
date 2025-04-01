using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class case2grabphoto : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(31.736f, 38.75f, 31.113f);  // 物品要回到的目標位置
    private Quaternion targetRotation = Quaternion.Euler(-90, 0, 180);  // 物品回到目標位置時的目標旋轉（根據需要修改）
    public float moveSpeed = 5f;      // 物品回到指定位置的速度
    public float rotationSpeed = 50000;  // 旋轉速度，控制物品旋轉的平滑度

    private XRGrabInteractable grabInteractable;  // 用來檢查物品是否被抓取
    private Rigidbody rb;  // 物品的 Rigidbody

    public GameObject missionbutton;
    public TextMeshProUGUI followtext;
    public float grabbedFontSize = 24;
    public AudioSource audioSource;  // 音源組件

    public whisper_texttospeech whisperScript;

    //private bool isgrab = false;


    void Start()
    {
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
            grabInteractable.onSelectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        // 如果物品沒有被抓取且不在目標位置，則回到目標位置
        if (grabInteractable != null && !grabInteractable.isSelected && transform.position != targetPosition)
        {
            MoveObjectBack();
        }
    }

    // 當玩家抓取物品時執行
    private void OnGrab(XRBaseInteractor interactor)
    {
        //isgrab = true;
        followtext.text = "請複誦\n你看\n這些都是我們一家人的大合照\n擺在客廳十幾年了\n這裡是你的家\n這裡很安全";
        followtext.fontSize = grabbedFontSize;
        
        if (audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // 停止當前播放的音頻
            }
            audioSource.Play(); // 這樣音檔只會在「抓取時」播放，而不會在 Update 中每幀執行
            StartCoroutine(WaitForAudioFinish());
        }
        //whisperScript.StartRecording();
        //missionbutton.SetActive(true);
    }

    private void OnRelease(XRBaseInteractor interactor)
    {
        //isgrab = false;
        
        if (whisperScript != null)
        {
            whisperScript.StopRecording();  // 停止語音辨識
        }

        Debug.Log("物品被放下，語音辨識停止！");
    }

    private IEnumerator WaitForAudioFinish()
    {
        // 等待音頻播放結束
        yield return new WaitForSeconds(audioSource.clip.length);

        // 音頻播放結束後開始錄音辨識
        whisperScript.StartRecording(); // 假設 StartRecording 是 whisper_texttospeech 中的開始錄音方法
    }

    // 讓物品回到指定位置並保持正確的方向
    void MoveObjectBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnReachedTargetPosition()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.position == targetPosition)
        {
            OnReachedTargetPosition();
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.RemoveListener(OnGrab);
            grabInteractable.onSelectEntered.RemoveListener(OnRelease);
        }
    }
}
