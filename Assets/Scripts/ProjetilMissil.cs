using UnityEngine;

public class ProjetilMissil : MonoBehaviour
{
    [Header("Configurações do Projetil")]
    public float velocidade = 60f;
    public float tempoVida = 4f;

    [Header("Efeitos")]
    public GameObject efeitoExplosao;
    public AudioClip somExplosao;

    void Start()
    {
        Destroy(gameObject, tempoVida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        ObstaculoMovimento obstaculo = other.GetComponent<ObstaculoMovimento>();
        if (obstaculo != null || other.CompareTag("Obstaculo"))
        {
            Explodir();

            if (obstaculo != null)
            {
                obstaculo.DestruirObstaculo();
            }
            else
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void Explodir()
    {
        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
        }

        if (somExplosao != null)
        {
            AudioSource.PlayClipAtPoint(somExplosao, transform.position);
        }
    }
}