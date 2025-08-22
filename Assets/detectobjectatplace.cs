using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowButtonOnClick : MonoBehaviour
{
    public XRBaseInteractable interactable; // 需要能被點擊的物件（加 XR Grab Interactable 或 Simple Interactable）
    public Button buttonToShow;             // 要顯示的按鈕

    private void Start()
    {
        if (buttonToShow != null)
            buttonToShow.gameObject.SetActive(false); // 一開始隱藏

        if (interactable != null)
            interactable.selectEntered.AddListener(OnClicked);
    }

    private void OnClicked(SelectEnterEventArgs args)
    {
        if (buttonToShow != null)
        {
            buttonToShow.gameObject.SetActive(true);
            Debug.Log("✅ 物體被點擊，按鈕已顯示！");
        }
    }
}

