using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightAndSoundXR : MonoBehaviour
{
    public Light spotLight;
    public AudioSource audioSource;

    void Start()
    {
        // 一開始燈要關著
        if (spotLight != null)
        {
            spotLight.enabled = false;
        }
    }

    public void ToggleLightAndSound()
    {
        if (spotLight != null)
        {
            bool isOn = spotLight.enabled;
            spotLight.enabled = !isOn;

            if (!isOn && audioSource != null)
            {
                audioSource.Play(); // 燈剛打開時播放音效
            }
        }
    }
}



