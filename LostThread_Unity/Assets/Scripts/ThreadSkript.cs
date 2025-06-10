using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class ThreadToCamera : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    [Header("Thread Settings")]
    public int pointsCount = 20;
    public float threadWidth = 0.01f;

    [Header("Wave Animation")]
    public float waveAmplitude = 0.02f;
    public float waveFrequency = 2f;
    public float waveSpeed = 2f;

    [Header("Materials")]
    public Material MaterialBase;
    public Material MaterialThread1;
    public Material MaterialThread2;
    public Material MaterialThread3;

    [Header("Melody Detection")]
    public Transform melodyParent; // where to detect the sphere

    private LineRenderer line;
    private int melodyCount = 0;
    private int lastMelodyCount = 0;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = pointsCount;
        line.useWorldSpace = true;
        line.startWidth = threadWidth;
        line.endWidth = threadWidth;

        if (MaterialBase != null)
            line.sharedMaterial = MaterialBase;
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            Vector3 startPos = startPoint.position;
            Vector3 endPos = endPoint.position;
            Vector3 direction = endPos - startPos;

            // Перпендикуляр направлению нитки и вверх камеры
            Vector3 rightFromCam = Vector3.Cross(direction.normalized, Camera.main.transform.up).normalized;

            for (int i = 0; i < pointsCount; i++)
            {
                float t = i / (float)(pointsCount - 1);
                Vector3 basePos = Vector3.Lerp(startPos, endPos, t);

                float wave = Mathf.Sin(Time.time * waveSpeed + t * waveFrequency * Mathf.PI * 2);
                float falloff = Mathf.Sin(t * Mathf.PI);
                basePos += rightFromCam * wave * waveAmplitude * falloff;

                line.SetPosition(i, basePos);
            }
        }

        //// Обработка нажатий клавиш, keyword test
        //if (Keyboard.current != null)
        //{
        //    if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //        OnMelodyCollected("own.melody");

        //    if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //        OnMelodyCollected("own.melody2");

        //    if (Keyboard.current.digit3Key.wasPressedThisFrame)
        //        OnMelodyCollected("own.melody3");
        //}

        //// Применение базового материала, если ничего не собрано
        //if (melodyCount == 0 && MaterialBase != null && line.sharedMaterial != MaterialBase)
        //{
        //    line.sharedMaterial = MaterialBase;
        //}

        if (melodyParent != null)
        {
            int currentCount = melodyParent.childCount;

            if (currentCount != lastMelodyCount)
            {
                lastMelodyCount = currentCount;
                OnMelodyCountChanged(currentCount); 
            }
        }

    }

    // keyboard test
    //public void OnMelodyCollected(string melodyName)
    //{
    //    if (melodyName != "own.melody" && melodyName != "own.melody2" && melodyName != "own.melody3")
    //        return;

    //    melodyCount++;
    //    Debug.Log($"Мелодия получена: {melodyName}. Счётчик: {melodyCount}");

    //    switch (melodyCount)
    //    {
    //        case 1:
    //            if (MaterialThread1 != null)
    //                line.sharedMaterial = MaterialThread1;
    //            break;
    //        case 2:
    //            if (MaterialThread2 != null)
    //                line.sharedMaterial = MaterialThread2;
    //            break;
    //        case 3:
    //            if (MaterialThread3 != null)
    //                line.sharedMaterial = MaterialThread3;
    //            break;
    //        default:
    //            Debug.Log("Максимальное количество смен достигнуто");
    //            break;
    //    }
    //}

    private void OnMelodyCountChanged(int count)
    {
        melodyCount = count;
        Debug.Log($"melody number: {count}");

        switch (count)
        {
            case 0:
                if (MaterialBase != null)
                    line.sharedMaterial = MaterialBase;
                break;
            case 1:
                if (MaterialThread1 != null)
                    line.sharedMaterial = MaterialThread1;
                break;
            case 2:
                if (MaterialThread2 != null)
                    line.sharedMaterial = MaterialThread2;
                break;
            case 3:
                if (MaterialThread3 != null)
                    line.sharedMaterial = MaterialThread3;
                break;
            default:
                Debug.Log("more than 3");
                break;
        }
    }
}
