using UnityEngine;

public class TwoObjectClickTracker : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject nextButton;

    private bool object1Clicked = false;
    private bool object2Clicked = false;

    void Start()
    {
        if (nextButton != null)
            nextButton.SetActive(false);
    }

    // ✅ 確保這個方法存在
    public void OnObjectTriggered(GameObject obj)
    {
        if (obj == object1)
            object1Clicked = true;

        if (obj == object2)
            object2Clicked = true;

        if (object1Clicked && object2Clicked)
        {
            nextButton.SetActive(true);
        }
    }
}

