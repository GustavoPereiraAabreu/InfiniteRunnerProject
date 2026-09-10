using UnityEngine;

public class MovimentoJato : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidadeFrente = 20f;
    public float velocidadeLateral = 15f;

    [Header("Limites do Cenário")]
    public float limiteEsquerda = -10f;
    public float limiteDireita = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * velocidadeFrente * Time.deltaTime, Space.World);

        float inputHorizontal = Input.GetAxis("Horizontal");

        Vector3 posicaoAtual = transform.position;
        float novaPosicaoX = posicaoAtual.x + (inputHorizontal * velocidadeLateral * Time.deltaTime);

        posicaoAtual.x = Mathf.Clamp(novaPosicaoX, limiteEsquerda, limiteDireita);

        transform.position = posicaoAtual;
    }
}