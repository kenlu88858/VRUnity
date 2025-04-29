using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TranscriptionUploader : MonoBehaviour
{
    public string serverUrl = "http://192.168.137.27:5000/transcribe";

    public IEnumerator UploadWavFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, "recording.wav", "audio/wav");

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                Debug.Log("辨識結果：" + response);

                // TODO：這裡你可以觸發語音播放或 UI 顯示是否正確
            }
            else
            {
                Debug.LogError("上傳錯誤：" + www.error);
            }
        }
    }
}

