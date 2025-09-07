using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowCanvasOnSelect : MonoBehaviour
{
    public GameObject canvasToShow;
    private XRBaseInteractable interactable;

    void Awake()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false);

        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);
    }
}
