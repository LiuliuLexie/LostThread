using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class FinalSceneTrigger : MonoBehaviour
{
    public AudioClip melody1, melody2, melody3, melody4;
    public AudioSource audioSource;

    public GameObject finalCanvas;
    public TextMeshProUGUI finalText;
    public GameObject blackEnvironment;

    private bool melody1Collected = false;
    private bool melody2Collected = false;
    private bool melody3Collected = false;

    private bool melodiesCollected = false;
    private bool isFinalPlayed = false;

    void Start()
    {
        finalCanvas.SetActive(false);
        blackEnvironment.SetActive(false);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && !melodiesCollected)
        {
            if (keyboard.digit1Key.wasPressedThisFrame)
                melody1Collected = true;
            if (keyboard.digit2Key.wasPressedThisFrame)
                melody2Collected = true;
            if (keyboard.digit3Key.wasPressedThisFrame)
                melody3Collected = true;

            if (melody1Collected && melody2Collected && melody3Collected)
                TryActivateFinalScene();
        }

        // ??????? Backspace ????????? ?????
        if (keyboard != null && finalCanvas.activeSelf && keyboard.backspaceKey.wasPressedThisFrame && !isFinalPlayed)
        {
            PlayFinalMelody();
        }
    }

    void LateUpdate()
    {
        if (finalCanvas.activeSelf)
        {
            Transform cam = Camera.main.transform;
            finalCanvas.transform.position = cam.position + cam.forward * 0.5f;
            finalCanvas.transform.rotation = Quaternion.LookRotation(cam.forward);
        }
    }

    public void TryActivateFinalScene()
    {
        if (!melody1 || !melody2 || !melody3 || melodiesCollected) return;

        melodiesCollected = true;
        finalCanvas.SetActive(true);
        finalText.text = "You’ve recovered enough melodies.\r\nThe thread is complete\r\nYou can get your 'body' again...\r\n\r\nBut you know, are you still the same?\r\n\r\n\r\n";
    }

    public void OnContinueButtonPressed()
    {
        finalCanvas.SetActive(false);
    }

    public void OnEndButtonPressed()
    {
        PlayFinalMelody();
    }

    private void PlayFinalMelody()
    {
        blackEnvironment.SetActive(true);
        finalCanvas.SetActive(false);
        audioSource.clip = melody4;
        audioSource.Play();
        isFinalPlayed = true;

        // ????????? ???? ?????? ?? X = 12
        Transform camTransform = Camera.main.transform;
        Vector3 currentEuler = camTransform.eulerAngles;
        camTransform.eulerAngles = new Vector3(12f, currentEuler.y, currentEuler.z);
    }
}
