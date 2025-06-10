using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterFadeInEffect : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3, 10)] public string[] sentences;

    [Header("Настройки")]
    public float typingSpeed = 0.05f;
    public float pauseAfterSentence = 1.5f;
    public float fadeDuration = 0.5f;

    private int index = 0;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        while (index < sentences.Length)
        {
            yield return StartCoroutine(TypeSentenceWithFade(sentences[index]));
            yield return new WaitForSeconds(pauseAfterSentence);

            if (index == 4)
            {
                Debug.Log("Переход на сцену '3'");
                SceneManager.LoadScene("3");
                yield break;
            }

            index++;
        }
    }

    IEnumerator TypeSentenceWithFade(string sentence)
    {
        textDisplay.text = sentence;
        textDisplay.ForceMeshUpdate();
        TMP_TextInfo textInfo = textDisplay.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIndex = textInfo.characterInfo[i].vertexIndex;
            Color32[] newColors = textInfo.meshInfo[matIndex].colors32;

            for (int j = 0; j < 4; j++)
            {
                newColors[vertIndex + j].a = 0;
            }
        }
        textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            float t = 0;
            while (t < fadeDuration)
            {
                float alpha = Mathf.Lerp(0, 255, t / fadeDuration);
                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertIndex = textInfo.characterInfo[i].vertexIndex;
                Color32[] newColors = textInfo.meshInfo[matIndex].colors32;

                for (int j = 0; j < 4; j++)
                {
                    newColors[vertIndex + j].a = (byte)alpha;
                }

                textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                t += Time.deltaTime;
                yield return null;
            }

            int matIdx = textInfo.characterInfo[i].materialReferenceIndex;
            int vIdx = textInfo.characterInfo[i].vertexIndex;
            Color32[] finalColors = textInfo.meshInfo[matIdx].colors32;

            for (int j = 0; j < 4; j++)
            {
                finalColors[vIdx + j].a = 255;
            }

            textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}

//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LetterFadeInEffect : MonoBehaviour
//{
//    [Header("UI")]
//    public TextMeshProUGUI textDisplay;
//    [TextArea(3, 10)] public string[] sentences;

//    [Header("Voice Clips")]
//    public AudioClip[] sentenceAudioClips;         // 一句话一个clip
//    public AudioSource voiceAudioSource;           // 播放句子的声音

//    [Header("Ambient Sources")]
//    public AudioSource windSource;
//    public AudioSource waterSource;
//    public AudioSource musicSource;
//    public float ambientTargetVolume = 0.5f;       // 最终环境音音量
//    public float ambientFadeDuration = 1.5f;       // 每句fade in的时间

//    [Header("Settings")]
//    public float typingSpeed = 0.05f;
//    public float pauseAfterSentence = 1.5f;
//    public float fadeDuration = 0.5f;

//    private int index = 0;

//    void Start()
//    {
//        // 初始化环境音
//        SetAmbientVolume(0f);
//        windSource.Play();
//        waterSource.Play();
//        musicSource.Play();

//        StartCoroutine(PlaySequence());
//    }

//    IEnumerator PlaySequence()
//    {
//        while (index < sentences.Length)
//        {
//            // 播放当前句子的语音
//            if (index < sentenceAudioClips.Length && sentenceAudioClips[index] != null)
//            {
//                voiceAudioSource.clip = sentenceAudioClips[index];
//                voiceAudioSource.Play();
//            }

//            // 渐变环境音
//            StartCoroutine(FadeAmbientSound(index));

//            // 打字 & 渐隐字母
//            yield return StartCoroutine(TypeSentenceWithFade(sentences[index]));
//            yield return new WaitForSeconds(pauseAfterSentence);

//            if (index == 4)
//            {
//                SceneManager.LoadScene("3");
//                yield break;
//            }

//            index++;
//        }
//    }

//    IEnumerator FadeAmbientSound(int step)
//    {
//        float startWind = windSource.volume;
//        float startWater = waterSource.volume;
//        float startMusic = musicSource.volume;

//        float targetVolume = Mathf.Lerp(0f, ambientTargetVolume, (float)(step + 1) / sentences.Length);
//        float elapsed = 0f;

//        while (elapsed < ambientFadeDuration)
//        {
//            float t = elapsed / ambientFadeDuration;
//            float v = Mathf.Lerp(0f, targetVolume, t);

//            windSource.volume = Mathf.Lerp(startWind, v, t);
//            waterSource.volume = Mathf.Lerp(startWater, v, t);
//            musicSource.volume = Mathf.Lerp(startMusic, v, t);

//            elapsed += Time.deltaTime;
//            yield return null;
//        }

//        // 最终设定目标音量
//        windSource.volume = targetVolume;
//        waterSource.volume = targetVolume;
//        musicSource.volume = targetVolume;
//    }

//    void SetAmbientVolume(float volume)
//    {
//        windSource.volume = volume;
//        waterSource.volume = volume;
//        musicSource.volume = volume;
//    }

//    IEnumerator TypeSentenceWithFade(string sentence)
//    {
//        textDisplay.text = sentence;
//        textDisplay.ForceMeshUpdate();
//        TMP_TextInfo textInfo = textDisplay.textInfo;

//        for (int i = 0; i < textInfo.characterCount; i++)
//        {
//            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
//            int vertIndex = textInfo.characterInfo[i].vertexIndex;
//            Color32[] newColors = textInfo.meshInfo[matIndex].colors32;

//            for (int j = 0; j < 4; j++)
//                newColors[vertIndex + j].a = 0;
//        }

//        textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

//        for (int i = 0; i < textInfo.characterCount; i++)
//        {
//            if (!textInfo.characterInfo[i].isVisible) continue;

//            float t = 0;
//            while (t < fadeDuration)
//            {
//                float alpha = Mathf.Lerp(0, 255, t / fadeDuration);
//                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
//                int vertIndex = textInfo.characterInfo[i].vertexIndex;
//                Color32[] newColors = textInfo.meshInfo[matIndex].colors32;

//                for (int j = 0; j < 4; j++)
//                    newColors[vertIndex + j].a = (byte)alpha;

//                textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
//                t += Time.deltaTime;
//                yield return null;
//            }

//            int matIdx = textInfo.characterInfo[i].materialReferenceIndex;
//            int vIdx = textInfo.characterInfo[i].vertexIndex;
//            Color32[] finalColors = textInfo.meshInfo[matIdx].colors32;

//            for (int j = 0; j < 4; j++)
//                finalColors[vIdx + j].a = 255;

//            textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
//            yield return new WaitForSeconds(typingSpeed);
//        }
//    }
//}
