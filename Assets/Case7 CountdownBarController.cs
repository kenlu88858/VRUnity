using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
     public Image fillImage;
    private float duration;
    private float timer;
    private bool isCounting = false;

    public void StartCountdown(float seconds)
    {
        duration = seconds;
        timer = seconds;
        fillImage.fillAmount = 1f;
        isCounting = true;
        gameObject.SetActive(true);
    }

    public void StopCountdown()
    {
        isCounting = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isCounting)
        {
            timer -= Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / duration);

            if (timer <= 0f)
            {
                StopCountdown();
            }
        }
    }
}
