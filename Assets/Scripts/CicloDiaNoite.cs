using System.Collections;
using UnityEngine;

public class CicloDiaNoite : MonoBehaviour
{
    [Header("Configuração do Ciclo")]
    public float duracaoDoDiaEmSegundos = 240f;
    public float duracaoTransicao = 3f;

    [Header("Referência da Luz Principal")]
    public Light luzSol;

    [Header("Materiais do Skybox")]
    public Material skyboxDia;
    public Material skyboxNoite;

    [Header("Cores da Luz")]
    public Color corLuzDia = new Color(1f, 0.95f, 0.8f);
    public Color corLuzNoite = new Color(0.05f, 0.05f, 0.2f);

    private float cronometroTempo = 0f;
    private bool ehDia = true;
    private Coroutine corrotinaTransicao;

    void Start()
    {
        if (skyboxDia != null)
        {
            RenderSettings.skybox = skyboxDia;
        }
    }

    void Update()
    {
        float velocidadeRotacao = (360f / duracaoDoDiaEmSegundos) * Time.deltaTime;
        transform.Rotate(Vector3.right * velocidadeRotacao);

        cronometroTempo += Time.deltaTime;
        float metadeDoCiclo = duracaoDoDiaEmSegundos / 2f;

        if (ehDia && cronometroTempo >= metadeDoCiclo)
        {
            ehDia = false;
            IniciarTransicao(skyboxNoite, corLuzNoite, 0.1f);
        }
        else if (!ehDia && cronometroTempo >= duracaoDoDiaEmSegundos)
        {
            ehDia = true;
            cronometroTempo = 0f;
            IniciarTransicao(skyboxDia, corLuzDia, 1f);
        }

        if (luzSol != null && corrotinaTransicao == null)
        {
            float anguloX = transform.eulerAngles.x;
            float anguloRad = anguloX * Mathf.Deg2Rad;
            float fatorDia = Mathf.Clamp01(Mathf.Sin(anguloRad) * 2f);

            luzSol.color = Color.Lerp(corLuzNoite, corLuzDia, fatorDia);
            luzSol.intensity = Mathf.Lerp(0.1f, 1f, fatorDia);
        }
    }

    void IniciarTransicao(Material novoSkybox, Color corAlvoLuz, float intensidadeAlvoLuz)
    {
        if (corrotinaTransicao != null)
        {
            StopCoroutine(corrotinaTransicao);
        }

        corrotinaTransicao = StartCoroutine(EfeitoFadeSkybox(novoSkybox, corAlvoLuz, intensidadeAlvoLuz));
    }

    IEnumerator EfeitoFadeSkybox(Material novoSkybox, Color corAlvoLuz, float intensidadeAlvoLuz)
    {
        Material skyboxAntigo = RenderSettings.skybox;
        float tempo = 0f;

        Color corInicialLuz = luzSol != null ? luzSol.color : Color.white;
        float intensidadeInicialLuz = luzSol != null ? luzSol.intensity : 1f;

        while (tempo < duracaoTransicao / 2f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / (duracaoTransicao / 2f);

            if (skyboxAntigo != null && skyboxAntigo.HasProperty("_Exposure"))
            {
                skyboxAntigo.SetFloat("_Exposure", Mathf.Lerp(1f, 0.2f, progresso));
            }

            yield return null;
        }

        if (novoSkybox != null)
        {
            RenderSettings.skybox = novoSkybox;
            if (novoSkybox.HasProperty("_Exposure"))
            {
                novoSkybox.SetFloat("_Exposure", 0.2f);
            }
        }

        DynamicGI.UpdateEnvironment();
        tempo = 0f;

        while (tempo < duracaoTransicao / 2f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / (duracaoTransicao / 2f);

            if (novoSkybox != null && novoSkybox.HasProperty("_Exposure"))
            {
                novoSkybox.SetFloat("_Exposure", Mathf.Lerp(0.2f, 1f, progresso));
            }

            if (luzSol != null)
            {
                luzSol.color = Color.Lerp(corInicialLuz, corAlvoLuz, progresso);
                luzSol.intensity = Mathf.Lerp(intensidadeInicialLuz, intensidadeAlvoLuz, progresso);
            }

            yield return null;
        }

        if (novoSkybox != null && novoSkybox.HasProperty("_Exposure"))
        {
            novoSkybox.SetFloat("_Exposure", 1f);
        }

        corrotinaTransicao = null;
    }
}