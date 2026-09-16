using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarCena : MonoBehaviour
{
    public void Recarregar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}