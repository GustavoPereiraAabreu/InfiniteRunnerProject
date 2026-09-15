using UnityEngine;

public class ObstaculoMovimento : MonoBehaviour
{

    [Header("Configurações de Movimento")]
    public float velocidade = 25f;
    public float limiteZDestruicao = -15f;

    void Update()
    {
        transform.Translate(Vector3.back * velocidade * Time.deltaTime, Space.World);

        if (transform.position.z < limiteZDestruicao)
        {
            Destroy(gameObject);
        }
    }
}