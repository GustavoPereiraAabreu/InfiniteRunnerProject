using UnityEngine;

public class GeradorDeObstaculos : MonoBehaviour
{
    [Header("Prefabs de Obstáculos")]
    public GameObject[] prefabsObstaculos;

    [Header("Faixas do Jogo")]
    public Transform[] pontosFaixa;

    [Header("Configurações de Distância e Tempo")]
    public float distanciaSpawnZ = 80f;
    public float tempoInicial = 1f;
    public float intervaloEntreSpawns = 1.8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstaculo), tempoInicial, intervaloEntreSpawns);
    }

    void SpawnObstaculo()
    {
        if (prefabsObstaculos.Length == 0 || pontosFaixa.Length == 0) return;

        int faixaAleatoria = Random.Range(0, pontosFaixa.Length);

        int obstaculoAleatorio = Random.Range(0, prefabsObstaculos.Length);

        Vector3 posicaoSpawn = new Vector3(
            pontosFaixa[faixaAleatoria].position.x,
            pontosFaixa[faixaAleatoria].position.y,
            distanciaSpawnZ
        );

        Instantiate(prefabsObstaculos[obstaculoAleatorio], posicaoSpawn, Quaternion.identity);
    }
}