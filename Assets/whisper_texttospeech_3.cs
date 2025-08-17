using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;

public class whisper_texttospeech_3 : MonoBehaviour
{
    private string microphoneDevice;
    private Coroutine recordingCoroutine; // ��b���O�̤W��
    public string savePath;
    public string Targetsentence; //���n�[�W���I�Ÿ��B�Ů浥��
    public string saveFileName = "recordedAudio.wav";  // ���W�O�s���ɮצW

    public float recordDuration = 8f; // �����ɶ�
    public float waitTime = 2f; // �C�����ѫᵥ�ݮɶ�

    private bool isTrue = false;
    public GameObject nextbutton;
    public TextMeshProUGUI errorTipText; // ��ܿ��ѿ��~�����ܤ�r

    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource wrongAudioSource; // ���ѿ��~�ɭn�����y��

    public TextMeshProUGUI followtext;
    public TextMeshProUGUI followtext1;
    public GameObject recordingPanel; // ??�s�W�G�Τ@�޲z�˼ƿ�������� UI Panel
    public TextMeshProUGUI countdownTitleText;
    public Image recordingProgressBar;
    public TextMeshProUGUI countdownText;

    public float whis_FontSize;

    [TextArea]
    public string grab;

    [TextArea]
    public string grab1;

    [TextArea]
    public string recongnize;

    [TextArea]
    public string finish;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0]; // �ϥβĤ@�ӳ��J���]��
            savePath = Path.Combine(Application.persistentDataPath, saveFileName); // �]�w�O�s���|
            //StartCoroutine(RecordingLoop()); // �Ұʴ`������
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
        //一開始會隱藏讀條
        recordingPanel?.SetActive(false);
        countdownTitleText.gameObject.SetActive(false);
        recordingProgressBar.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
    }

    public void StartRecording()
    {
        if (recordingCoroutine != null)
        {
            Debug.LogWarning("啟動語音辨識");
            return;
        }
        recordingCoroutine = StartCoroutine(RecordingLoop());
    }

    private IEnumerator RecordingLoop()
    {
        while (!isTrue) // �L���`������
        {
            followtext.text = grab;
            followtext.fontSize = whis_FontSize;
            followtext1.text = grab1;
            followtext1.fontSize = whis_FontSize;
            while (audioSource.isPlaying)
            {
                yield return null;  // ���ݪ��쭵�W���񵲧�
            }

            Debug.Log("�ж}�l����...");
            //�}�l��������ܳѾl���ƩM��L�q��
            recordingPanel?.SetActive(true);
            countdownTitleText.gameObject.SetActive(true);
            recordingProgressBar.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(true);
            StartCoroutine(ShowRecordingCountdown(recordDuration));
            // �}�l����
            AudioClip recordedClip = Microphone.Start(microphoneDevice, false, (int)recordDuration, 44100);

            // ���� 10 ��
            yield return new WaitForSeconds(recordDuration);

            // �������
            Microphone.End(microphoneDevice);

            //���������ɱN�q������
            recordingPanel?.SetActive(false);
            countdownTitleText.gameObject.SetActive(false);
            recordingProgressBar.gameObject.SetActive(false);
            countdownText.gameObject.SetActive(false);


            errorTipText.gameObject.SetActive(false);
            followtext.text = recongnize;
            followtext.fontSize = whis_FontSize;
            followtext1.text = "";
            followtext1.fontSize = whis_FontSize;
            Debug.Log("�y�����s�����A�}�l����...");

            // �O�s���W�ɮ�
            audioSource1.Play();
            SaveAudioClipAsWav(recordedClip, savePath);
            Debug.Log("WAV �ɮ��x�s��: " + savePath);

            // �o�e���W����A������
            yield return StartCoroutine(SendAudioToServer(savePath));

            // ���� 2 ���A�~��U�@������
            yield return new WaitForSeconds(waitTime);
        }

        followtext.text = finish;
        followtext.fontSize = whis_FontSize;
        followtext1.text = "";
        followtext1.fontSize = whis_FontSize;
        nextbutton.SetActive(true);
        Debug.Log("��������A�y�����Ѥw�����C");
        recordingCoroutine = null;
        StopRecording();
    }
    //showrecordingcountdown�O�s�W��function
    private IEnumerator ShowRecordingCountdown(float duration)
    {
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            // ��s���ιϱq�k�����]�O�o Image �]�w FillOrigin=Right�^
            float fillAmount = timeLeft / duration;
            recordingProgressBar.fillAmount = fillAmount;

            // ��s�˼Ƥ�r
            countdownText.text = Mathf.CeilToInt(timeLeft).ToString() + " ��";

            yield return null;
        }

        recordingProgressBar.fillAmount = 0;
        countdownText.text = "0 ��";
    }

    // �N���s�����W�O�s�� WAV �ɮ�
    public static void SaveAudioClipAsWav(AudioClip clip, string path)
    {
        byte[] audioData = WavUtility2.FromAudioClip(clip); // �ഫ AudioClip �� WAV �榡�� byte[]
        File.WriteAllBytes(path, audioData); // �x�s�ɮ�
    }

    // �o�e���W�ɮר���A��
    private IEnumerator SendAudioToServer(string audioFilePath)
    {
        string serverUrl = "http://127.0.0.1:5000/transcribe";  // ���A���� URL
        WWWForm form = new WWWForm();
        byte[] audioData = File.ReadAllBytes(audioFilePath);  // Ū�����W�ɮ�

        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");  // ���]�ɮ׬O WAV �榡

        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string rawText = www.downloadHandler.text;
            Debug.Log("���A���^��: " + rawText);

            string extractedText = ExtractTextFromJson(rawText);

            string cleanedText = RemovePunctuationAndWhitespace(extractedText);
            if (Targetsentence == cleanedText)
            {
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop();
                }
                Debug.Log("�A����F!");
                audioSource2.Play();
                isTrue = true;
                errorTipText.gameObject.SetActive(false); // ���T�����ÿ��~�T��
                nextbutton.SetActive(true);  // ? ��o��[�b�o�̡I

            }
            else
            {
                errorTipText.gameObject.SetActive(true); // ��ܥX��
                // ?? ���~�᭫�s��� grab �M grab1 ??
                followtext.text = grab;
                followtext.fontSize = whis_FontSize;
                followtext1.text = grab1;
                followtext1.fontSize = whis_FontSize;
                Debug.Log("�����W�I");
                wrongAudioSource.Play();
                yield return new WaitWhile(() => wrongAudioSource.isPlaying);
                //yield return new WaitForSeconds(audioSource.clip.length);
                // ��ܿ��~���ܤ�r
                // errorTipText.text = "���Ѥ��e���~";
                // errorTipText.fontSize = whis_FontSize; // ��쥻�@�P
            }

            Debug.Log("�y�����ѵ��G: " + cleanedText);
        }
        else
        {
            Debug.LogError("���ѿ��~: " + www.error);
        }
    }

    private string ExtractTextFromJson(string jsonText)
    {
        try
        {
            var jsonObj = JsonUtility.FromJson<ResponseData>(jsonText);
            return jsonObj.text; // ���]���A���^�Ǯ榡�� {"text": "�A�n�A�o�O���աI"}
        }
        catch
        {
            Debug.LogError("�L�k�ѪR JSON�A�нT�{���A���^���榡");
            return jsonText; // �p�G�ѪR���ѡA�����^�ǭ�l�r��
        }
    }

    private string RemovePunctuationAndWhitespace(string input)
    {
        return Regex.Replace(input, @"\W+", ""); // \W+ ���ܲ����Ҧ��D�r���Ʀr���r��
    }

    [System.Serializable]
    private class ResponseData
    {
        public string text;
    }

    public void StopRecording()
    {
        if (recordingCoroutine != null)
        {
            StopCoroutine(recordingCoroutine);
            recordingCoroutine = null;
        }

        if (Microphone.IsRecording(microphoneDevice))
        {
            Microphone.End(microphoneDevice); // �קK�b�S�����ɥs End �X��
        }
        Debug.Log("�����y�{�w��ʰ���I");
    }
}
