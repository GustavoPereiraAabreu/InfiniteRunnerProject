using UnityEngine;
using UnityEngine.InputSystem;

public class PausarJogo : MonoBehaviour
{
    [Header("Painel de Pause")]
    [SerializeField] private GameObject painelPause;

    private bool estaPausado = false;

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        if (Keyboard.current != null &&
           (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (estaPausado)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        estaPausado = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (painelPause != null)
        {
            painelPause.SetActive(true);
        }
    }

    public void Continuar()
    {
        estaPausado = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (painelPause != null)
        {
            painelPause.SetActive(false);
        }
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}