using UnityEngine;

public class ObstaculoMovimento : MonoBehaviour
{
    [Header("Velocidade do Mundo")]
    public float velocidadeInicial = 25f;
    public float velocidadeMaxima = 60f;
    public float ganhoVelocidadePorSegundo = 0.5f;

    [Header("Destruição Automática")]
    public bool destruirAoPassar = false;
    public float limiteZDestruicao = -20f;

    private static float velocidadeAtual;

    void Start()
    {
        if (velocidadeAtual < velocidadeInicial)
        {
            velocidadeAtual = velocidadeInicial;
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
            Destroy(gameObject);
        }
    }
}