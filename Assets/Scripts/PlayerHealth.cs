using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidas = 3;
    private int vidasRestantes;

    private string tagObstaculo = "Obstaculo";

    [Header("Tempo de Proteção pós-Dano")]
    public int tempoInvulneravel = 30;
    private bool isInvulneravel = false;

    [Header("Sistema de Escudo")]
    public bool temEscudoAtivo = false;
    public GameObject efeitoVisualEscudo;
    public AudioClip somColetarEscudo;
    public AudioClip somEscudoProtegeu;
    private Coroutine corrotinaEscudo;

    [Header("Efeito Visual e UI")]
    public Renderer meshRendererNave;
    public GameObject painelGameOver;

    [Header("UI de Vidas e Distância Fictícia")]
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoDistancia;
    public TextMeshProUGUI textoDistanciaGameOver;

    [Header("Configuração dos Metros Fakes")]
    public float multiplicadorMetros = 10f;
    private float distanciaPercorrida = 0f;

    [Header("Efeitos Sonoros")]
    public AudioSource audioSource;
    public AudioClip somDano;
    public AudioClip somMorte;

    void Start()
    {
        vidasRestantes = vidas;
        AtualizarTextoVidas();

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(false);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!CanvasStart.jogoIniciado || vidasRestantes <= 0) return;

        distanciaPercorrida += Time.deltaTime * multiplicadorMetros;
        AtualizarTextoDistancia();
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
        if (!CanvasStart.jogoIniciado) return;

        if (objetoColidido.CompareTag(tagObstaculo) || objetoColidido.GetComponent<ObstaculoMovimento>() != null)
        {
            if (temEscudoAtivo)
            {
                TocarSom(somEscudoProtegeu);

                Destroy(objetoColidido);
                return;
            }

            if (isInvulneravel) return;

            TomarDano(1);
            Destroy(objetoColidido);
        }
    }

    public void AtivarEscudo(float duracao)
    {
        TocarSom(somColetarEscudo);

        if (corrotinaEscudo != null)
        {
            StopCoroutine(corrotinaEscudo);
        }

        corrotinaEscudo = StartCoroutine(RotinaTemporizadorEscudo(duracao));
    }

    private IEnumerator RotinaTemporizadorEscudo(float duracao)
    {
        temEscudoAtivo = true;

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(true);
        }

        yield return new WaitForSeconds(duracao);

        DesativarEscudo();
    }

    private void DesativarEscudo()
    {
        temEscudoAtivo = false;

        if (efeitoVisualEscudo != null)
        {
            efeitoVisualEscudo.SetActive(false);
        }

        if (corrotinaEscudo != null)
        {
            StopCoroutine(corrotinaEscudo);
            corrotinaEscudo = null;
        }
    }

    public void TomarDano(int quantidade)
    {
        vidasRestantes -= quantidade;
        AtualizarTextoVidas();

        if (vidasRestantes <= 0)
        {
            vidasRestantes = 0;
            TocarSom(somMorte != null ? somMorte : somDano, usarAudio3DNoPonto: true);
            Morrer();
        }
        else
        {
            TocarSom(somDano);
            StartCoroutine(RotinaInvulnerabilidade());
        }
    }

    private void AtualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasRestantes;
        }
    }

    private void AtualizarTextoDistancia()
    {
        if (textoDistancia != null)
        {
            textoDistancia.text = Mathf.FloorToInt(distanciaPercorrida) + " m";
        }
    }

    private void TocarSom(AudioClip clip, bool usarAudio3DNoPonto = false)
    {
        if (clip == null) return;

        if (usarAudio3DNoPonto)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        else if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void Morrer()
    {
        CanvasStart.PararJogo();

        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (textoDistanciaGameOver != null)
        {
            textoDistanciaGameOver.text = "Distância: " + Mathf.FloorToInt(distanciaPercorrida) + " m";
        }

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }

        Destroy(gameObject);
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