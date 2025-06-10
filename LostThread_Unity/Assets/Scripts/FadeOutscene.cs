using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // ?????? UI-Image, ?????????? ?? ???? ?????
    public float fadeDuration = 1f;

    public void StartFadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScene(sceneName));
    }

    private IEnumerator FadeAndSwitchScene(string sceneName)
    {
        float t = 0f;
        Color color = fadeImage.color;

        // Fade Out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }

        // ???????? ?????? ?????
        SceneManager.LoadScene(sceneName);
        Debug.Log("?????? ???????? ?? ????? 3");
        // FindObjectOfType<SceneFader>().StartFadeToScene("3");

    }
}
