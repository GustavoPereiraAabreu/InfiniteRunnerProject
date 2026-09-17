using UnityEngine;

public class PausarJogo : MonoBehaviour
{
    [Header("Painel de Pause")]
    [SerializeField] private GameObject painelPause;

    public void Pausar()
    {
        Time.timeScale = 0f;

        if (painelPause != null)
        {
            painelPause.SetActive(true);
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1f;

        if (painelPause != null)
        {
            painelPause.SetActive(false);
        }
    }
}
