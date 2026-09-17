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

    [Header("Aleatoriedade de Tempo")]
    public float variacaoTempo = 0.3f;

    [Header("Dificuldade Progressiva")]
    public float taxaAceleracao = 0.05f;

    private float intervaloSpawnAtual;
    private float proximoIntervalo;
    private float cronometro = 0f;
    private bool gerando = false;

    private int ultimaFaixa = -1;

    void Start()
    {
        intervaloSpawnAtual = intervaloSpawnInicial;
        CalcularProximoIntervalo();
    }

    public void IniciarGeracao()
    {
        gerando = true;
        cronometro = 0f;
        CalcularProximoIntervalo();
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

        if (cronometro >= proximoIntervalo)
        {
            SpawnObstaculo();
            cronometro = 0f;
            CalcularProximoIntervalo();
        }
    }

    void CalcularProximoIntervalo()
    {
        float offset = Random.Range(-variacaoTempo, variacaoTempo);
        proximoIntervalo = Mathf.Max(intervaloSpawnMinimo, intervaloSpawnAtual + offset);
    }

    void SpawnObstaculo()
    {
        if (prefabsObstaculos.Length == 0 || pontosFaixa.Length == 0) return;

        int faixaSorteada;
        if (pontosFaixa.Length > 1)
        {
            do
            {
                faixaSorteada = Random.Range(0, pontosFaixa.Length);
            } while (faixaSorteada == ultimaFaixa);
        }
        else
        {
            faixaSorteada = 0;
        }

        ultimaFaixa = faixaSorteada;

        int obstaculoSorteado = Random.Range(0, prefabsObstaculos.Length);

        Vector3 posicaoSpawn = new Vector3(
            pontosFaixa[faixaSorteada].position.x,
            pontosFaixa[faixaSorteada].position.y,
            posicaoZSpawn
        );

        GameObject obstaculo = Instantiate(prefabsObstaculos[obstaculoSorteado], posicaoSpawn, Quaternion.identity);
        obstaculo.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }
}