using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class case2loadgarden : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("case2-5_Garden");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("case2-6");
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene("again");
    }
}
