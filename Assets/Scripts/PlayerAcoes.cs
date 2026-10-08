using System.Collections;
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
    public AudioSource audioSourceEscudo;

    [Header("Sistema de Escudo")]
    public GameObject efeitoVisualEscudo;
    private bool estaProtegido = false;
    private Coroutine corrotinaEscudo;

    void Start()
    {
        if (audioSourceDisparo == null)
        {
            audioSourceDisparo = GetComponent<AudioSource>();
        }

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(false);
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

    public void AtivarEscudo(float duracao)
    {
        if (corrotinaEscudo != null)
        {
            StopCoroutine(corrotinaEscudo);
        }

        corrotinaEscudo = StartCoroutine(RotinaEscudo(duracao));
    }

    private IEnumerator RotinaEscudo(float duracao)
    {
        estaProtegido = true;

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(true);
        }

        if (audioSourceEscudo != null)
        {
            audioSourceEscudo.Play();
        }

        yield return new WaitForSeconds(duracao);

        estaProtegido = false;

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(false);
        }
    }

    public bool VerificarEscudoAbsorveuDano()
    {
        if (estaProtegido)
        {
            return true;
        }

        return false;
    }
}