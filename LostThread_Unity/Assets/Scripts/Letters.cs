using TMPro;
using UnityEngine;
using System.Collections;

public class TypeAndDustSequence : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float letterDelay = 0.05f;
    public float pauseAfterText = 1f;

    private string[] texts = new string[]
    {
        "Привет!",
        "Как дела?",
        "Пока.",
        "Пошел в жопу." // или измени как хочешь :)
    };

    void Start()
    {
        StartCoroutine(PlayAllTexts());
    }

    IEnumerator PlayAllTexts()
    {
        foreach (string t in texts)
        {
            yield return StartCoroutine(TypeText(t));
            yield return new WaitForSeconds(pauseAfterText);
            yield return StartCoroutine(SandFadeEffect(t));
            yield return new WaitForSeconds(0.5f);
        }

        textMesh.text = ""; // очистить в конце
    }

    IEnumerator TypeText(string fullText)
    {
        textMesh.text = "";
        yield return null; // подождать 1 кадр, чтобы TextMeshPro обновился

        for (int i = 0; i < fullText.Length; i++)
        {
            textMesh.text += fullText[i];
            yield return new WaitForSeconds(letterDelay);
        }

        textMesh.ForceMeshUpdate();
    }

    IEnumerator SandFadeEffect(string fullText)
    {
        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;

        float[] charTimers = new float[textInfo.characterCount];
        bool[] done = new bool[textInfo.characterCount];

        float duration = 1.5f;
        float delayBetweenChars = 0.03f;

        for (int i = 0; i < charTimers.Length; i++)
            charTimers[i] = i * delayBetweenChars;

        while (true)
        {
            textMesh.ForceMeshUpdate();
            textInfo = textMesh.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible || done[i]) continue;

                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[matIndex].vertices;
                Color32[] colors = textInfo.meshInfo[matIndex].colors32;

                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), -1f * Time.deltaTime, 0f);
                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += offset;
                }

                float t = charTimers[i] / duration;
                byte alpha = (byte)Mathf.Lerp(255, 0, t);
                for (int j = 0; j < 4; j++)
                {
                    colors[vertexIndex + j].a = alpha;
                }

                charTimers[i] += Time.deltaTime;
                if (charTimers[i] >= duration) done[i] = true;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            if (System.Array.TrueForAll(done, b => b)) yield break;

            yield return null;
        }
    }
}
