using UnityEngine;
using TMPro;

public class ChildInteraction1 : MonoBehaviour
{
    [Header("Matching Logic")]
    public int requiredMelodyID;
    public WindChimeBackpack1 backpack;

    [Header("Gaze UI")]
    public TextMeshProUGUI gazeText;

    [Header("Texts")]
    public string successText = "Thank you. I remembered... just a little.";
    public string failureText = "This is not my melody...";

    [Header("Exchange Feedback")]
    public GameObject childLight; 
    public Transform sphereTargetAttachPoint;

    //[Header("Child Transform (optional override)")]
    //public Transform childTransform;

    [Header("Post-Exchange Movement")]
    public Transform chimeAttachPoint;
    public Transform childWaitSpace;

    [Header("Child Sound")]
    //public AudioSource soundSource;
    public AudioSource dialogue;
    public AudioSource thankyou;
    public AudioSource fullMelody;
    public AudioSource singleMelody;


    private bool hasReceivedChime = false;

    public void TriggerDialogue()
    {
        if (hasReceivedChime) return;

        GameObject matchingChime = backpack.FindAttachedChime(requiredMelodyID);
        Debug.Log($"Looking for chime with melody ID: {requiredMelodyID}");

        if (matchingChime != null)
        {
            Debug.Log("Found matching chime: " + matchingChime.name);
            hasReceivedChime = true;

            // ShowText(successText);
            GazeRaycaster1.instance.gazeText.text = successText;
            StartCoroutine(CompleteExchangeAndFly(matchingChime));

        }
        else
        {
            Debug.Log("No matching chime found.");
            // ShowText(failureText);
            GazeRaycaster1.instance.gazeText.text = failureText;
        }
    }

    // private void ShowText(string message)
    // {
    //     if (gazeText != null)
    //     {
    //         gazeText.text = message;
    //         gazeText.gameObject.SetActive(true);
    //         Invoke(nameof(HideText), 2f);
    //     }
    // }

    private void HideText()
    {
        if (gazeText != null)
        {
            gazeText.gameObject.SetActive(false);
        }
    }

    //private Transform GetChildTransform()
    //{
    //    return childTransform != null ? childTransform : transform;
    //}


    private System.Collections.IEnumerator MoveChimeToChild(GameObject chime)
    {
        float duration = 3f;
        float time = 0f;
        Vector3 start = chime.transform.position;
        Vector3 end = transform.position + Vector3.up * 0.2f; // 偏上位置，避免穿模

        while (time < duration)
        {
            chime.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }


        chime.transform.position = end;
        chime.transform.SetParent(this.transform);
    }

    private System.Collections.IEnumerator MoveChildSphereToAttachPoint()
    {
        if (childLight == null || sphereTargetAttachPoint == null)
        {
            Debug.LogWarning("Missing childLight or sphereTargetAttachPoint reference.");
            yield break;
        }

        float duration = 3f;
        float time = 0f;
        Vector3 start = childLight.transform.position;
        Vector3 end = sphereTargetAttachPoint.position;

        while (time < duration)
        {
            childLight.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        childLight.transform.position = end;
        childLight.transform.SetParent(sphereTargetAttachPoint);
    }

    //private System.Collections.IEnumerator FlyAwayWithChime(GameObject chime)
    //{
    //    if (childWaitSpace == null)
    //    {
    //        Debug.LogWarning("Missing childWaitSpace reference.");
    //        yield break;
    //    }

    //    float duration = 40f;
    //    float floatDuration = 5f; // 起伏只持续前5秒
    //    float time = 0f;

    //    Vector3 start = transform.position;
    //    Vector3 end = childWaitSpace.position;
    //    Vector3 controlPoint = (start + end) / 2f + Vector3.down * 1.5f;

    //    Vector3 chimeOffset = Vector3.down * 0.25f;

    //    // 随机的轻微水平漂移
    //    float horizontalWiggleRange = 0.3f;
    //    float horizontalOffset = Random.Range(-horizontalWiggleRange, horizontalWiggleRange);
    //    Vector3 lateralOffset = new Vector3(horizontalOffset, 0f, 0f);

    //    while (time < duration)
    //    {
    //        float t = time / duration;
    //        float easedT = t * t * (3f - 2f * t); // EaseInOut

    //        // 贝塞尔路径（带少量横向偏移）
    //        Vector3 bezierPos =
    //            Mathf.Pow(1 - easedT, 2) * start +
    //            2 * (1 - easedT) * easedT * (controlPoint + lateralOffset) +
    //            Mathf.Pow(easedT, 2) * end;

    //        // 轻微浮动：1~2次慢速起伏
    //        float floatStrength = Mathf.Clamp01(1 - (time / floatDuration));
    //        float floatOffsetY = Mathf.Sin(time * Mathf.PI * 0.5f) * 0.2f * floatStrength; // 慢速，幅度小
    //        Vector3 floatingOffset = Vector3.up * floatOffsetY;

    //        //Transform target = GetChildTransform();

    //        // 设置位置
    //        transform.position = bezierPos + floatingOffset;
    //        //chime.transform.position = bezierPos + chimeOffset + floatingOffset;

    //        // 轻微旋转
    //        transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    //        //chime.transform.Rotate(Vector3.up, 20f * Time.deltaTime);

    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    // 精准落点
    //    transform.position = end;
    //    chime.transform.position = end + chimeOffset;
    //}

private System.Collections.IEnumerator FlyAwayWithChime(GameObject chime)
{
    if (childWaitSpace == null)
    {
        Debug.LogWarning("Missing childWaitSpace reference.");
        yield break;
    }

    float duration = 40f;
    float floatDuration = 5f;
    float time = 0f;

    Vector3 start = transform.position;
    Vector3 end = childWaitSpace.position;
    Vector3 controlPoint = (start + end) / 2f + Vector3.down * 1.5f;

    // 随机的轻微水平漂移
    float horizontalWiggleRange = 0.3f;
    float horizontalOffset = Random.Range(-horizontalWiggleRange, horizontalWiggleRange);
    Vector3 lateralOffset = new Vector3(horizontalOffset, 0f, 0f);

    while (time < duration)
    {
        float t = time / duration;
        float easedT = t * t * (3f - 2f * t); // EaseInOut

        Vector3 bezierPos =
            Mathf.Pow(1 - easedT, 2) * start +
            2 * (1 - easedT) * easedT * (controlPoint + lateralOffset) +
            Mathf.Pow(easedT, 2) * end;

        float floatStrength = Mathf.Clamp01(1 - (time / floatDuration));
        float floatOffsetY = Mathf.Sin(time * Mathf.PI * 0.5f) * 0.2f * floatStrength;
        Vector3 floatingOffset = Vector3.up * floatOffsetY;

        transform.position = bezierPos + floatingOffset;
        transform.Rotate(Vector3.up, 10f * Time.deltaTime);

        time += Time.deltaTime;
        yield return null;
    }

    transform.position = end;

    if (chimeAttachPoint != null)
    {
        Vector3 downwardOffset = new Vector3(0f, -0.25f, 0f); // 让风铃位于孩子模型下方一点
        chime.transform.position = chimeAttachPoint.position + downwardOffset;
        chime.transform.rotation = chimeAttachPoint.rotation;
        // chime.transform.SetParent(chimeAttachPoint); // 可选：挂上去
    }
}




    private System.Collections.IEnumerator CompleteExchangeAndFly(GameObject chime)
    {
        yield return StartCoroutine(MoveChimeToChild(chime));

        dialogue.gameObject.SetActive(false);
        singleMelody.gameObject.SetActive(false);
        thankyou.gameObject.SetActive(true);

        childLight.gameObject.SetActive(true);
        yield return StartCoroutine(MoveChildSphereToAttachPoint());

        fullMelody.gameObject.SetActive(true);

        yield return StartCoroutine(FlyAwayWithChime(chime));

        //if (dialogue != null)
        //    dialogue.Stop();
    }




    void OnGazeEnter()
    {
        GazeRaycaster1.instance.gazeText.text = "Can you ...hear me?";
    }
}
