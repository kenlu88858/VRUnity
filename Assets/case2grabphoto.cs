using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class case2grabphoto : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(32.66f, 39.04f, 30.89f);  // 物品要回到的目標位置
    private Quaternion targetRotation = Quaternion.Euler(-90, 0, 180);  // 物品回到目標位置時的目標旋轉（根據需要修改）
    public float moveSpeed = 5f;      // 物品回到指定位置的速度
    public float rotationSpeed = 50000;  // 旋轉速度，控制物品旋轉的平滑度

    private XRGrabInteractable grabInteractable;  // 用來檢查物品是否被抓取
    private Rigidbody rb;  // 物品的 Rigidbody

    public GameObject missionbutton;
    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;
    public float grabbedFontSize = 24;
    public AudioSource audioSource;  // 音源組件
    public AudioSource audioSource1;

    public case2_whisper_texttospeech whisperScript;
    private bool recongnize_true = false;

    public Transform photoframe;

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
            //grabInteractable.onSelectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        // 如果物品沒有被抓取且不在目標位置，則回到目標位置
        /* if (grabInteractable != null && !grabInteractable.isSelected && transform.position != targetPosition)
        {
            MoveObjectBack();
        } */

        Vector3 PhotoPosition = photoframe.position;
        Debug.Log("photoframe Position: " + PhotoPosition);
    }

    // 當玩家抓取物品時執行
    private void OnGrab(XRBaseInteractor interactor)
    {
        //isgrab = true;
        //rb.isKinematic = false;
        //rb.useGravity = false;
        texttospeech();
        /* grabInteractable.enabled = false;
        MoveObjectBack();

        if(!recongnize_true)
        {
            followtext.text = "請和我複誦一次以下文字\n\n\n\n\n\n\n請開始複誦";
            followtext.fontSize = grabbedFontSize;
            followtext1.text = "\n你看\n這些都是我們一家人的大合照\n擺在客廳十幾年\n這裡是你的家\n這裡很安全";
            followtext1.fontSize = grabbedFontSize;

            if (audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop(); // 停止當前播放的音頻
                }
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop(); // 停止當前播放的音頻
                }
                audioSource.Play(); // 這樣音檔只會在「抓取時」播放，而不會在 Update 中每幀執行
                StartCoroutine(WaitForAudioFinish());
            }
        } */
        
        
        //whisperScript.StartRecording();
        //missionbutton.SetActive(true);
    }

    private void texttospeech()
    {
        grabInteractable.enabled = false;
        StartCoroutine(MoveObjectBack());
        if(!recongnize_true)
        {
            followtext.text = "請和我複誦一次以下文字\n\n\n\n\n\n\n請開始複誦";
            followtext.fontSize = grabbedFontSize;
            followtext1.text = "\n你看\n這些都是我們一家人的大合照\n擺在客廳十幾年\n這裡是你的家\n這裡很安全";
            followtext1.fontSize = grabbedFontSize;

            if (audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop(); // 停止當前播放的音頻
                }
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop(); // 停止當前播放的音頻
                }
                audioSource.Play(); // 這樣音檔只會在「抓取時」播放，而不會在 Update 中每幀執行
                StartCoroutine(WaitForAudioFinish());
            }
        }
    }

    /* private void OnRelease(XRBaseInteractor interactor)
    {
        //isgrab = false;
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // 停止當前播放的音頻
        }
        
        if (whisperScript != null)
        {
            if (File.Exists(whisperScript.savePath))
            {
                try
                {
                    File.Delete(whisperScript.savePath);
                    Debug.Log("舊的音檔已刪除: " + whisperScript.savePath);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("刪除舊音檔失敗: " + e.Message);
                }
            }
            whisperScript.StopRecording();  // 停止語音辨識
        }
        if(!recongnize_true)
        {
            followtext.text = "請操作手柄\n\n拿起全家福照片";
            followtext.fontSize = grabbedFontSize;
            followtext1.text = "";
            followtext1.fontSize = grabbedFontSize;
        }

        Debug.Log("物品被放下，語音辨識停止！");
    } */

    private IEnumerator WaitForAudioFinish()
    {
        // 等待音頻播放結束
        yield return new WaitForSeconds(audioSource.clip.length);

        // 音頻播放結束後開始錄音辨識
        whisperScript.StartRecording(); // 假設 StartRecording 是 whisper_texttospeech 中的開始錄音方法
        recongnize_true = true;
    }

    // 讓物品回到指定位置並保持正確的方向
    IEnumerator MoveObjectBack()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.0f){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    /* private void OnReachedTargetPosition()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    } */

/*     private void OnTriggerEnter(Collider other)
    {
        if (transform.position == targetPosition)
        {
            OnReachedTargetPosition();
        }
    } */

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.RemoveListener(OnGrab);
            //grabInteractable.onSelectEntered.RemoveListener(OnRelease);
        }
    }
}
