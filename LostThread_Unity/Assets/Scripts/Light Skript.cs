using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light pointLight;
    public float maxIntensity = 5f;
    public float minIntensity = 0.5f;
    public float flickerSpeed = 0.1f;
    public float triggerDistance = 5f;
    public Transform player;

    private float originalIntensity;
    private bool isPlayerNearby = false;

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
        originalIntensity = pointLight.intensity;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerNearby = distance <= triggerDistance;

        if (isPlayerNearby)
        {
            pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * flickerSpeed, 1));
        }
        else
        {
            pointLight.intensity = originalIntensity;
        }
    }
}
