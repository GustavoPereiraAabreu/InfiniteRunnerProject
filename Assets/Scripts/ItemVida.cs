using UnityEngine;

public class ItemVida : MonoBehaviour
{
    [Header("Configuração de Cura")]
    public int quantidadeVidaParaCurar = 1;

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        transform.Translate(0, 0, -ObstaculoMovimento.velocidadeAtual * Time.deltaTime, Space.World);

        if (transform.position.z < -20f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.AdicionarVida(quantidadeVidaParaCurar);
            }

            Destroy(gameObject);
        }
    }
}