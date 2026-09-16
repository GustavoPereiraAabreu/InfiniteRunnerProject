using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidas = 3;
    private int vidasRestantes;

    private string tagObstaculo = "Obstaculo";

    [Header("Tempo de Proteção pós-Dano")]
    public float tempoInvulneravel = 1.5f;
    private bool isInvulneravel = false;

    [Header("Efeito Visual e UI")]
    public Renderer meshRendererNave;
    public GameObject painelGameOver;

    void Start()
    {
        vidasRestantes = vidas;
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        VerificarColisao(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        VerificarColisao(collision.gameObject);
    }

    void VerificarColisao(GameObject objetoColidido)
    {
        if (!CanvasStart.jogoIniciado || isInvulneravel) return;

        if (objetoColidido.CompareTag(tagObstaculo) || objetoColidido.GetComponent<ObstaculoMovimento>() != null)
        {
            TomarDano(1);

            Destroy(objetoColidido);
        }
    }

    public void TomarDano(int quantidade)
    {
        vidasRestantes -= quantidade;

        if (vidasRestantes <= 0)
        {
            vidasRestantes = 0;
            Morrer();
        }
        else
        {
            StartCoroutine(RotinaInvulnerabilidade());
        }
    }

    private void Morrer()
    {

        CanvasStart.PararJogo();

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    IEnumerator RotinaInvulnerabilidade()
    {
        isInvulneravel = true;

        if (meshRendererNave != null)
        {
            float tempoPassado = 0f;
            while (tempoPassado < tempoInvulneravel)
            {
                meshRendererNave.enabled = !meshRendererNave.enabled;
                yield return new WaitForSeconds(0.1f);
                tempoPassado += 0.1f;
            }
            meshRendererNave.enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(tempoInvulneravel);
        }

        isInvulneravel = false;
    }
}