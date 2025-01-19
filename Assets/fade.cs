using UnityEngine;

public class TextFader : MonoBehaviour
{
    public CanvasGroup canvasGroup; 
    public float fadeDuration = 2f; 
    public float displayTime = 3f; 

    private void Start()
    {
        StartCoroutine(FadeOutAfterDelay());
    }

    private System.Collections.IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
