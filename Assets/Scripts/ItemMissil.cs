using UnityEngine;

public class ItemMissil : MonoBehaviour
{
    [Header("Configurações do Item")]
    public float velocidadeRotacao = 100f;
    public float limiteZDestruicao = -20f;

    [Header("Efeitos")]
    public AudioClip somColeta;

    void Update()
    {
        if (!CanvasStart.jogoIniciado) return;

        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime, Space.World);

        transform.Translate(Vector3.back * ObstaculoMovimento.velocidadeAtual * Time.deltaTime, Space.World);

        if (transform.position.z < limiteZDestruicao)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<PlayerAcoes>() != null)
        {
            PlayerAcoes player = other.GetComponentInParent<PlayerAcoes>();
            if (player != null)
            {
                player.ColetarEDispararMissil();
            }

            if (somColeta != null)
            {
                AudioSource.PlayClipAtPoint(somColeta, transform.position);
            }

            Destroy(gameObject);
        }
    }
}