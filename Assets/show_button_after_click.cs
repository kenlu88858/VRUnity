using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections; // 確保引用 Coroutine 需要的命名空間

// 類別名稱已修改，以更符合其功能
public class ShowButtonOnSelect : MonoBehaviour
{
    [Header("要顯示的按鈕")]
    // 修改：變數名稱從 canvasToShow 改為 buttonToShow，使其更具描述性
    public GameObject buttonToShow;

    [Header("跳躍效果")]
    public float jumpHeight = 0.2f;   // 跳躍高度
    public float jumpDuration = 0.3f; // 跳躍時間

    private XRBaseInteractable interactable;
    private bool hasBeenSelected = false;

    void Awake()
    {
        // 修改：在遊戲開始時，隱藏指定的按鈕
        if (buttonToShow != null)
            buttonToShow.SetActive(false);

        interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelected);
        }
        else
        {
            Debug.LogError("錯誤：這個物件上找不到 XRBaseInteractable 元件！", this.gameObject);
        }
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (hasBeenSelected) return; // 已經觸發過就不再觸發

        hasBeenSelected = true;

        // 修改：當物件被選取時，顯示按鈕
        if (buttonToShow != null)
            buttonToShow.SetActive(true);

        // 播放跳躍效果
        StartCoroutine(JumpEffect());

        // 禁用 XR 互動，避免再次被選取
        interactable.enabled = false;
    }

    private IEnumerator JumpEffect()
    {
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        float elapsed = 0f;

        // 上升
        while (elapsed < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(startPos, peakPos, elapsed / (jumpDuration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 到頂
        transform.position = peakPos;

        elapsed = 0f;

        // 下降
        while (elapsed < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(peakPos, startPos, elapsed / (jumpDuration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 回到原位
        transform.position = startPos;
    }
}
