using UnityEngine;

public class GlassMaterialChanger : MonoBehaviour
{
    [Header("Transparency Settings")]
    [Range(0f, 1f)] public float transparency = 0.5f;
    public Color glassColor = new Color(1f, 1f, 1f, 0.5f);

    private Material originalMaterial;
    private Material glassMaterial;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            originalMaterial = renderer.material;
            glassMaterial = new Material(Shader.Find("Standard"));

            // Set the material to Transparent mode
            glassMaterial.SetFloat("_Mode", 3);
            glassMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            glassMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            glassMaterial.SetInt("_ZWrite", 0);
            glassMaterial.DisableKeyword("_ALPHATEST_ON");
            glassMaterial.EnableKeyword("_ALPHABLEND_ON");
            glassMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            glassMaterial.renderQueue = 3000;

            // Set the color with transparency
            glassColor.a = transparency;
            glassMaterial.color = glassColor;

            renderer.material = glassMaterial;
        }
    }
}
