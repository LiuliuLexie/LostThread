using UnityEngine;

public class EnvironmentAudioManager : MonoBehaviour
{
    public AudioSource windAudio;
    public AudioSource riverAudio;
    public AudioSource musicAudio;

    [Header("Fade Settings")]
    public float fadeInDuration = 300f;
    public float targetVolumeWind = 1f;
    public float targetVolumeRiver = 1f;
    public float targetVolumeMusic = 1f;

    private float timeElapsed = 0f;
    private bool fading = true;
    private static bool exists = false;
    private static bool alreadyInitialized = false;

    void Awake()
    {
        if (exists)
        {
            Destroy(gameObject); // 避免重复
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            exists = true;
        }
    }

    void Start()
    {
        if (alreadyInitialized) return; // 如果已经初始化过，就跳过

        if (windAudio != null) { windAudio.volume = 0f; windAudio.Play(); }
        if (riverAudio != null) { riverAudio.volume = 0f; riverAudio.Play(); }
        if (musicAudio != null) { musicAudio.volume = 0f; musicAudio.Play(); }

        alreadyInitialized = true;
    }

    void Update()
    {
        if (!fading) return;

        timeElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(timeElapsed / fadeInDuration);

        if (windAudio != null) windAudio.volume = Mathf.Lerp(0f, targetVolumeWind, t);
        if (riverAudio != null) riverAudio.volume = Mathf.Lerp(0f, targetVolumeRiver, t);
        if (musicAudio != null) musicAudio.volume = Mathf.Lerp(0f, targetVolumeMusic, t);

        if (t >= 1f) fading = false;
    }

    public void DestroyMusicOnly()
    {
        if (musicAudio != null)
        {
            Destroy(musicAudio.gameObject);
        }
    }
}
