using UnityEngine;

public class GeradorObstaculos : MonoBehaviour
{
    [Header("Prefabs dos Obstáculos")]
    public GameObject[] prefabsObstaculos;

    [Header("Faixas (Posições X e Y)")]
    public Transform[] pontosFaixa;

    [Header("Configurações de Spawn Inicial")]
    public float posicaoZSpawn = 100f;
    public float intervaloSpawnInicial = 2.5f;
    public float intervaloSpawnMinimo = 0.6f; 

    [Header("Dificuldade Progressiva")]
    public float taxaAceleracao = 0.05f; 

    private float intervaloSpawnAtual;
    private float cronometro = 0f;
    private bool gerando = false;

    void Start()
    {

        intervaloSpawnAtual = intervaloSpawnInicial;
    }


    public void IniciarGeracao()
    {
        gerando = true;
        cronometro = 0f; 
    }

    void Update()
    {
        if (!gerando || !CanvasStart.jogoIniciado) return;

        if (intervaloSpawnAtual > intervaloSpawnMinimo)
        {
            intervaloSpawnAtual -= taxaAceleracao * Time.deltaTime;
            intervaloSpawnAtual = Mathf.Max(intervaloSpawnAtual, intervaloSpawnMinimo);
        }

        cronometro += Time.deltaTime;

        if (cronometro >= intervaloSpawnAtual)
        {
            SpawnObstaculo();
            cronometro = 0f;
        }
    }

    void SpawnObstaculo()
    {
        if (prefabsObstaculos.Length == 0 || pontosFaixa.Length == 0) return;

        int faixaSorteada = Random.Range(0, pontosFaixa.Length);
        int obstaculoSorteado = Random.Range(0, prefabsObstaculos.Length);

        Vector3 posicaoSpawn = new Vector3(
            pontosFaixa[faixaSorteada].position.x,
            pontosFaixa[faixaSorteada].position.y,
            posicaoZSpawn
        );

        Instantiate(prefabsObstaculos[obstaculoSorteado], posicaoSpawn, Quaternion.identity);
    }
}