using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CanvasStart : MonoBehaviour
{
    [Header("HUD / UI")]
    public CanvasGroup startText;
    public GameObject painelMenuInicial;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
        {
            playerController.SetActive(false);
        }
    }

    void Update()
    {
        if (!started)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartGame();
            }

            else if (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame))
            {
                StartGame();
            }
        }
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

        if (painelMenuInicial != null)
        {
            painelMenuInicial.SetActive(false);
        }

        yield return StartCoroutine(MoveCameraToPlayer());

        if (playerController != null)
        {
            playerController.SetActive(true);
        }

        if (introCamera != null)
        {
            introCamera.gameObject.SetActive(false);
        }

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
            if (startText != null)
            {
                startText.alpha = Mathf.Lerp(1f, 0f, t / textFadeDuration);
            }
            yield return null;
        }

        if (startText != null)
        {
            startText.alpha = 0f;
            startText.blocksRaycasts = false;
        }
    }

    IEnumerator MoveCameraToPlayer()
    {
        if (introCamera != null && playerCameraTarget != null)
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
}