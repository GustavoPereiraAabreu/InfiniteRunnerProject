using UnityEngine;

public class ObstaculoMovimento : MonoBehaviour
{
    [Header("Velocidade do Mundo")]
    public float velocidadeInicial = 25f;
    public float velocidadeMaxima = 60f;
    public float ganhoVelocidadePorSegundo = 0.5f;

    [Header("Destruição Automática")]
    public bool destruirAoPassar = true;
    public float limiteZDestruicao = -20f;

    [Header("Efeitos Sonoros")]
    public AudioSource audioSource;

    private static float velocidadeAtual;

    void Start()
    {
        if (velocidadeAtual < velocidadeInicial)
        {
            velocidadeAtual = velocidadeInicial;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        if (velocidadeAtual < velocidadeMaxima)
        {
            velocidadeAtual += ganhoVelocidadePorSegundo * Time.deltaTime;
        }

        transform.Translate(0, 0, -velocidadeAtual * Time.deltaTime, Space.World);

        if (destruirAoPassar && transform.position.z < limiteZDestruicao)
        {
            DestruirObstaculo();
        }
    }

    public void DestruirObstaculo()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Destroy(gameObject);
    }
}