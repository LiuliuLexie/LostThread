using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThreadMaterialController : MonoBehaviour
{
    public Material[] materials; // сюда в инспекторе добавь 3 материала

    private LineRenderer line;
    private int currentIndex = 0;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (materials.Length > 0)
            line.sharedMaterial = materials[0];
    }

    void OnMouseDown()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        line.sharedMaterial = materials[currentIndex];

        Debug.Log($"Материал сменился на: {materials[currentIndex].name}");
    }
}
