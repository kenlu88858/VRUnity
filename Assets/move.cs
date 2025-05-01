using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveOnSelect : MonoBehaviour
{
    public Transform targetPosition; // 指定要移動到的位置
    public float moveSpeed = 2f;     // 移動速度

    private bool shouldMove = false;

    void Start()
    {
        // 綁定 Select 事件
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    void Update()
    {
        if (shouldMove && targetPosition != null)
        {
            // 插值移動到目標位置
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // 如果到達位置，停止移動
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                shouldMove = false;
            }
        }
    }

    void OnSelected(SelectEnterEventArgs args)
    {
        shouldMove = true;
    }
}

