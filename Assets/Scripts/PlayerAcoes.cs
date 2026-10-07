using UnityEngine;

public class PlayerAcoes : MonoBehaviour
{
    [Header("Configuração do Disparo")]
    public GameObject prefabMissilProjetil;
    public Transform pontoDisparo;

    [Header("Ajuste de Rotação Visual")]
    public Vector3 rotacalAdicionalEuler = new Vector3(-90f, 0f, 0f);

    [Header("Efeitos Sonoros")]
    public AudioSource audioSourceDisparo;

    void Start()
    {
        if (audioSourceDisparo == null)
        {
            audioSourceDisparo = GetComponent<AudioSource>();
        }
    }

    public void ColetarEDispararMissil()
    {
        if (!CanvasStart.jogoIniciado) return;

        Vector3 posicaoSpawn = pontoDisparo != null ? pontoDisparo.position : transform.position + transform.forward * 2f;

        Quaternion rotacaoCorreta = Quaternion.Euler(rotacalAdicionalEuler);

        if (prefabMissilProjetil != null)
        {
            Instantiate(prefabMissilProjetil, posicaoSpawn, rotacaoCorreta);
        }

        if (audioSourceDisparo != null && audioSourceDisparo.clip != null)
        {
            audioSourceDisparo.Play();
        }
    }
}