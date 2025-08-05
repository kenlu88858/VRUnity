
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveOnSelect : MonoBehaviour
{
    public Transform targetPosition; // ���w�n���ʨ쪺��m
    public float moveSpeed = 2f;     // ���ʳt��

    private bool shouldMove = false;

    void Start()
    {
        // �j�w Select �ƥ�
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    void Update()
    {
        if (shouldMove && targetPosition != null)
        {
            // ���Ȳ��ʨ�ؼЦ�m
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // �p�G��F��m�A�����
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

