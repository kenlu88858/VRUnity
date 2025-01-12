using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultipleChoiceManager2 : MonoBehaviour
{
    public Button[] optionButtons;
    public Button confirmButton;
    public static bool[] selectedOptions = new bool[6];
    public static int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }

        confirmButton.onClick.AddListener(OnConfirm);
    }

    void OnOptionSelected(int index)
    {
        selectedOptions[index] = !selectedOptions[index];
        Debug.Log("Option " + (index + 1) + " selected: " + selectedOptions[index]);
    }

    void OnConfirm()
    {
        currentIndex = 0;
        Debug.Log("Confirm pressed, loading next selected scene.");
        LoadNextSelectedScene();
    }

    public void LoadNextSelectedScene()
    {
        while (currentIndex < selectedOptions.Length)
        {
            if (selectedOptions[currentIndex])
            {
                string sceneName = "case2-" + (currentIndex + 1);
                Debug.Log("Loading scene: " + sceneName);
                SceneManager.LoadScene(sceneName);
                return;
            }
            currentIndex++;
        }

        Debug.Log("All selected scenes complete, loading again.");
        ResetSelections();
        SceneManager.LoadScene("again");
    }

    public void OnNextButtonPressed()
    {
        currentIndex++;
        LoadNextSelectedScene();
    }

    void ResetSelections()
    {
        for (int i = 0; i < selectedOptions.Length; i++)
        {
            selectedOptions[i] = false;
        }
        currentIndex = 0;
    }
}
