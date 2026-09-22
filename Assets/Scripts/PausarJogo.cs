using UnityEngine;

public class PausarJogo : MonoBehaviour
{
    [Header("Painel de Pause")]
    [SerializeField] private GameObject painelPause;

    public void Pausar()
    {
        Time.timeScale = 0f;

        AudioListener.pause = true;

        if (painelPause != null)
        {
            painelPause.SetActive(true);
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1f;

        AudioListener.pause = false;

        if (painelPause != null)
        {
            painelPause.SetActive(false);
        }
    }
}