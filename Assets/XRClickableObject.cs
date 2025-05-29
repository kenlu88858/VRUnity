using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRClickableObject : MonoBehaviour
{
    public TwoObjectClickTracker tracker;

    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
        {
            Debug.LogError("需要 XRBaseInteractable 元件！");
            return;
        }

        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (tracker != null)
            tracker.OnObjectTriggered(this.gameObject);
    }
}
