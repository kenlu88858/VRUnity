using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabFix : MonoBehaviour
{
    private Vector3 originalScale; // 記錄原始大小

    private void Start()
    {
        originalScale = transform.localScale; // 存下物件的初始大小
    }

    private void OnEnable()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDisable()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        transform.localScale = originalScale; // 抓取時強制回到原本大小
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        transform.localScale = originalScale; // 釋放時維持原始大小
    }
}