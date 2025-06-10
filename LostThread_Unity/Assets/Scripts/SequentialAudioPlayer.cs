using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SequentialAudioPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float delayBetweenClips = 2f;

    [Header("Next Scene")]
    public string nextSceneName = "NextScene"; // Inspector中填入目标场景名

    void Start()
    {
        if (audioClips.Length > 0 && audioSource != null)
        {
            StartCoroutine(PlayClipsInSequence());
        }
        else
        {
            Debug.LogWarning("请在Inspector中设置AudioSource和AudioClips！");
        }
    }

    private IEnumerator PlayClipsInSequence()
    {
        foreach (AudioClip clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length + delayBetweenClips);
        }

        // 播完后，通知环境管理器销毁音乐，并切换场景
        EnvironmentAudioManager envManager = FindObjectOfType<EnvironmentAudioManager>();
        if (envManager != null)
        {
            envManager.DestroyMusicOnly();
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
