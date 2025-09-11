using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowCanvasOnSelect : MonoBehaviour
{
    [Header("Target UI")]
    public GameObject canvasToShow;

    [Header("Jump Effect")]
    public float jumpHeight = 0.2f;   // 跳躍高度
    public float jumpDuration = 0.3f; // 跳躍時間

    private XRBaseInteractable interactable;
    private bool hasBeenSelected = false;

    void Awake()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false);

        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (hasBeenSelected) return; // 已經觸發過就不再觸發

        hasBeenSelected = true;

        // 顯示 Canvas
        if (canvasToShow != null)
            canvasToShow.SetActive(true);

        // 播放跳躍效果
        StartCoroutine(JumpEffect());

        // 禁用 XR 互動，避免再次被選取
        interactable.enabled = false;
    }

    private System.Collections.IEnumerator JumpEffect()
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
