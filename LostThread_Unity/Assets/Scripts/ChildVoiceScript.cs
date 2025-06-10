using UnityEngine;
using System.Collections;

public class TriggerChildWithAudio : MonoBehaviour
{
    public GameObject childObject;
    public AudioSource childAudioSource;

    private Material childMat;
    private Coroutine fadeCoroutine;

    void Start()
    {
        childMat = childObject.GetComponent<Renderer>().material;
        Color c = childMat.color;
        c.a = 0;
        childMat.color = c;
        childObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            childObject.SetActive(true);
            fadeCoroutine = StartCoroutine(FadeToAlpha(1f));

            // ?????? ?????
            if (childAudioSource != null && !childAudioSource.isPlaying)
            {
                childAudioSource.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeToAlpha(0f));

            // ????????? ?????
            if (childAudioSource != null && childAudioSource.isPlaying)
            {
                childAudioSource.Stop();
            }
        }
    }

    IEnumerator FadeToAlpha(float targetAlpha)
    {
        float duration = 1.5f;
        float time = 0f;
        Color startColor = childMat.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (time < duration)
        {
            time += Time.deltaTime;
            childMat.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }

        childMat.color = endColor;

        if (targetAlpha == 0f)
        {
            childObject.SetActive(false);
        }
    }
}
