using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasStart : MonoBehaviour
{
    [Header("HUD")]
    public CanvasGroup startText;

    [Header("Cameras")]
    public Camera introCamera;
    public Transform playerCameraTarget;

    [Header("Player")]
    public GameObject playerController;

    [Header("Config")]
    public float textFadeDuration = 1f;
    public float cameraMoveDuration = 2f;

    public static bool jogoIniciado { get; private set; } = false;

    private bool started = false;

    void Awake()
    {
        jogoIniciado = false;
    }

    void Start()
    {
        playerController.SetActive(false);
    }

    public void StartGame()
    {
        if (started) return;

        started = true;
        StartCoroutine(GameSequence());
    }

    public static void PararJogo()
    {
        jogoIniciado = false;
    }

    IEnumerator GameSequence()
    {
        yield return StartCoroutine(FadeOutText());
        yield return StartCoroutine(MoveCameraToPlayer());

        playerController.SetActive(true);
        introCamera.gameObject.SetActive(false);

        jogoIniciado = true;

        GeradorObstaculos gerador = FindFirstObjectByType<GeradorObstaculos>();
        if (gerador != null)
        {
            gerador.IniciarGeracao();
        }
    }

    IEnumerator FadeOutText()
    {
        float t = 0f;

        while (t < textFadeDuration)
        {
            t += Time.deltaTime;
            startText.alpha = Mathf.Lerp(1f, 0f, t / textFadeDuration);
            yield return null;
        }

        startText.alpha = 0f;
    }

    IEnumerator MoveCameraToPlayer()
    {
        Vector3 startPos = introCamera.transform.position;
        Quaternion startRot = introCamera.transform.rotation;

        float t = 0f;

        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float progress = t / cameraMoveDuration;

            introCamera.transform.position = Vector3.Lerp(startPos, playerCameraTarget.position, progress);
            introCamera.transform.rotation = Quaternion.Slerp(startRot, playerCameraTarget.rotation, progress);

            yield return null;
        }

        introCamera.transform.position = playerCameraTarget.position;
        introCamera.transform.rotation = playerCameraTarget.rotation;
    }
}