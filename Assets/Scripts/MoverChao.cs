using UnityEngine;

public class MoverChao : MonoBehaviour
{
    [Header("Configuração de Velocidade")]
    public float multiplicadorVelocidade = 1.2f;

    [Header("Configuração de Loop")]
    public float tamanhoDoBloco = 100f;

    [Header("Quantos blocos existem em sequência")]
    public int quantidadeDeBlocos = 3;

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        float velocidadeFinal = ObstaculoMovimento.velocidadeAtual * multiplicadorVelocidade;
        transform.Translate(0, 0, -velocidadeFinal * Time.deltaTime, Space.World);

        
        if (transform.position.z <= -tamanhoDoBloco)
        {
            float deslocamentoTotal = tamanhoDoBloco * quantidadeDeBlocos;
            transform.Translate(0, 0, deslocamentoTotal, Space.World);
        }
    }
}