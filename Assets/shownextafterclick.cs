using UnityEngine;
using UnityEngine.Video;

public class VideoButtonController6 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject[] optionButtons;      // �e6�ӫ��s
    public GameObject confirmButton;        // ��7�ӫ��s
    public AudioClip instructionVoice;      // ���ܻy��

    public GameObject text1;                // �B�~�� Text 1
    public GameObject text2;                // �B�~�� Text 2
    public GameObject plane1;               // �B�~�� Plane 1
    public GameObject plane2;               // �B�~�� Plane 2

    private AudioSource audioSource;
    private bool hasSelectedOption = false;

    void Start()
    {
        // �������Ҧ����s
        foreach (GameObject btn in optionButtons)
            btn.SetActive(false);

        confirmButton.SetActive(false);

        // �����B�~����r�P����
        if (text1 != null) text1.SetActive(false);
        if (text2 != null) text2.SetActive(false);
        if (plane1 != null) plane1.SetActive(false);
        if (plane2 != null) plane2.SetActive(false);

        // ���U callback
        videoPlayer.loopPointReached += OnVideoFinished;

        // ���ļ���
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // ��ܫe6�ӿﶵ���s
        foreach (GameObject btn in optionButtons)
            btn.SetActive(true);

        // ����B�~������
        if (text1 != null) text1.SetActive(true);
        if (text2 != null) text2.SetActive(true);
        if (plane1 != null) plane1.SetActive(true);
        if (plane2 != null) plane2.SetActive(true);

        // ���񴣥ܻy��
        if (instructionVoice != null)
        {
            audioSource.PlayOneShot(instructionVoice);
        }
    }

    public void OnOptionSelected()
    {
        if (!hasSelectedOption)
        {
            confirmButton.SetActive(true);
            hasSelectedOption = true;
        }
    }
}