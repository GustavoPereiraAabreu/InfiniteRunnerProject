using UnityEngine;

public class ItemEscudo : MonoBehaviour
{
    public float duracaoEscudo = 5f;
    public float limiteZDestruicao = -20f;

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        transform.Translate(0, 0, -ObstaculoMovimento.velocidadeAtual * Time.deltaTime, Space.World);

        if (transform.position.z < limiteZDestruicao)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.AtivarEscudo(duracaoEscudo);
            }

            Destroy(gameObject);
        }
    }
}