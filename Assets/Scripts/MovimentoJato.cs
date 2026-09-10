using UnityEngine;
using UnityEngine.InputSystem;

public class JatoTrocaFaixa : MonoBehaviour
{
    [Header("Pontos de Posição (Faixas)")]
    public Transform[] pontosFaixa;

    [Header("Configurações de Movimento")]
    public float velocidadeTransicao = 20f;
    public bool usarTeleporteDireto = false;

    [Header("Animação / Componente Animator")]
    public Animator animatorJato;

    private int indiceFaixaAtual = 1;

    void Start()
    {
        if (pontosFaixa.Length > indiceFaixaAtual && pontosFaixa[indiceFaixaAtual] != null)
        {
            transform.position = pontosFaixa[indiceFaixaAtual].position;
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVetor = value.Get<Vector2>();

        if (inputVetor.x < -0.3f)
        {
            MudarFaixa(-1);
        }

        else if (inputVetor.x > 0.3f)
        {
            MudarFaixa(1);
        }
    }

    void MudarFaixa(int direcao)
    {
        int novoIndice = Mathf.Clamp(indiceFaixaAtual + direcao, 0, pontosFaixa.Length - 1);

        if (novoIndice != indiceFaixaAtual)
        {
            indiceFaixaAtual = novoIndice;

            if (animatorJato != null)
            {
                if (direcao < 0)
                {
                    animatorJato.SetTrigger("VirarEsquerda");
                }
                else if (direcao > 0)
                {
                    animatorJato.SetTrigger("VirarDireita");
                }
            }

            if (usarTeleporteDireto)
            {
                transform.position = pontosFaixa[indiceFaixaAtual].position;
            }
        }
    }

    void Update()
    {
        if (!usarTeleporteDireto && pontosFaixa.Length > 0)
        {
            Vector3 alvo = pontosFaixa[indiceFaixaAtual].position;
            transform.position = Vector3.MoveTowards(transform.position, alvo, velocidadeTransicao * Time.deltaTime);
        }
    }
}