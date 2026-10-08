using UnityEngine;

public class GeradorObstaculos : MonoBehaviour
{
    [Header("Prefabs dos Obstáculos")]
    public GameObject[] prefabsObstaculos;

    [Header("Item Coletável Míssil")]
    public GameObject prefabItemMissil;
    [Range(0f, 1f)] public float chanceSpawnMissil = 0.2f;

    [Header("Item Coletável Escudo")]
    public GameObject prefabItemEscudo;
    [Range(0f, 1f)] public float chanceSpawnEscudo = 0.15f;

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

    private int ultimaFaixaObstaculo = -1;

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
            SpawnObjeto();
            cronometro = 0f;
            CalcularProximoIntervalo();
        }
    }

    void CalcularProximoIntervalo()
    {
        float offset = Random.Range(-variacaoTempo, variacaoTempo);
        proximoIntervalo = Mathf.Max(intervaloSpawnMinimo, intervaloSpawnAtual + offset);
    }

    void SpawnObjeto()
    {
        if (pontosFaixa.Length == 0) return;

        int faixaObstaculo;
        if (pontosFaixa.Length > 1)
        {
            do
            {
                faixaObstaculo = Random.Range(0, pontosFaixa.Length);
            } while (faixaObstaculo == ultimaFaixaObstaculo);
        }
        else
        {
            faixaObstaculo = 0;
        }

        ultimaFaixaObstaculo = faixaObstaculo;

        if (prefabsObstaculos.Length > 0)
        {
            int obstaculoSorteado = Random.Range(0, prefabsObstaculos.Length);
            Vector3 posObstaculo = new Vector3(
                pontosFaixa[faixaObstaculo].position.x,
                pontosFaixa[faixaObstaculo].position.y,
                posicaoZSpawn
            );

            GameObject obstaculo = Instantiate(prefabsObstaculos[obstaculoSorteado], posObstaculo, Quaternion.identity);
            obstaculo.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }

        bool missilGerado = false;
        int faixaMissil = -1;

        if (prefabItemMissil != null && pontosFaixa.Length > 1 && Random.value <= chanceSpawnMissil)
        {
            do
            {
                faixaMissil = Random.Range(0, pontosFaixa.Length);
            } while (faixaMissil == faixaObstaculo);

            Vector3 posMissil = new Vector3(
                pontosFaixa[faixaMissil].position.x,
                pontosFaixa[faixaMissil].position.y,
                posicaoZSpawn
            );

            GameObject item = Instantiate(prefabItemMissil, posMissil, Quaternion.identity);
            item.transform.localRotation = Quaternion.identity;
            missilGerado = true;
        }

        if (prefabItemEscudo != null && pontosFaixa.Length > 1 && Random.value <= chanceSpawnEscudo)
        {
            int faixaEscudo;
            int tentativas = 0;
            do
            {
                faixaEscudo = Random.Range(0, pontosFaixa.Length);
                tentativas++;
            } while ((faixaEscudo == faixaObstaculo || (missilGerado && faixaEscudo == faixaMissil)) && tentativas < 10);

            if (faixaEscudo != faixaObstaculo && (!missilGerado || faixaEscudo != faixaMissil))
            {
                Vector3 posEscudo = new Vector3(
                    pontosFaixa[faixaEscudo].position.x,
                    pontosFaixa[faixaEscudo].position.y,
                    posicaoZSpawn
                );

                GameObject escudoItem = Instantiate(prefabItemEscudo, posEscudo, Quaternion.identity);
                escudoItem.transform.localRotation = Quaternion.identity;
            }
        }
    }
}