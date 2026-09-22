using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoJato : MonoBehaviour
{
    [Header("Pontos de Posição (Faixas)")]
    public Transform[] pontosFaixa;

    [Header("Configurações de Movimento")]
    public float velocidadeTransicao = 20f;
    public bool usarTeleporteDireto = false;

    private int indiceFaixaAtual = 1;

    void Start()
    {
        if (pontosFaixa != null && pontosFaixa.Length > indiceFaixaAtual && pontosFaixa[indiceFaixaAtual] != null)
        {
            transform.position = pontosFaixa[indiceFaixaAtual].position;
        }
    }

    public void MoverParaEsquerda()
    {
        MudarFaixa(-1);
    }

    public void MoverParaDireita()
    {
        MudarFaixa(1);
    }

    void MudarFaixa(int direcao)
    {
        if (pontosFaixa == null || pontosFaixa.Length == 0) return;

        int novoIndice = Mathf.Clamp(indiceFaixaAtual + direcao, 0, pontosFaixa.Length - 1);

        if (novoIndice != indiceFaixaAtual)
        {
            indiceFaixaAtual = novoIndice;

            if (usarTeleporteDireto)
            {
                transform.position = pontosFaixa[indiceFaixaAtual].position;
            }
        }
    }

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                MoverParaEsquerda();
            }
            if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                MoverParaDireita();
            }
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                MoverParaEsquerda();
            }
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                MoverParaDireita();
            }
        }

        if (!usarTeleporteDireto && pontosFaixa != null && pontosFaixa.Length > 0 && pontosFaixa[indiceFaixaAtual] != null)
        {
            Vector3 alvo = pontosFaixa[indiceFaixaAtual].position;
            transform.position = Vector3.MoveTowards(transform.position, alvo, velocidadeTransicao * Time.deltaTime);
        }
    }
}