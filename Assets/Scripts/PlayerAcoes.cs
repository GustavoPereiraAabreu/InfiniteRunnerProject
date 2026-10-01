using UnityEngine;

public class PlayerAcoes : MonoBehaviour
{
    [Header("Configuração do Disparo")]
    public GameObject prefabMissilProjetil;
    public Transform pontoDisparo;

    [Header("Efeitos")]
    public AudioClip somDisparo;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ColetarEDispararMissil()
    {
        if (!CanvasStart.jogoIniciado) return;

        Vector3 posicaoSpawn = pontoDisparo != null ? pontoDisparo.position : transform.position + transform.forward * 2f;
        Quaternion rotacaoSpawn = pontoDisparo != null ? pontoDisparo.rotation : transform.rotation;

        if (prefabMissilProjetil != null)
        {
            Instantiate(prefabMissilProjetil, posicaoSpawn, rotacaoSpawn);
        }

        if (somDisparo != null && audioSource != null)
        {
            audioSource.PlayOneShot(somDisparo);
        }
    }
}