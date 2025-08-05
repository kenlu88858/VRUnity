using UnityEngine;
using UnityEngine.UI;

public class ShowButtonAfterDelay3 : MonoBehaviour
{
    public Button targetButton;     // �n��ܪ����s
    public float delayTime = 3f;    // ����X����X�{

    private void Start()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false); // �@�}�l������
            Invoke(nameof(ShowButton), delayTime);    // �������
        }
    }

    private void ShowButton()
    {
        targetButton.gameObject.SetActive(true); // ��ܫ��s
    }
}
