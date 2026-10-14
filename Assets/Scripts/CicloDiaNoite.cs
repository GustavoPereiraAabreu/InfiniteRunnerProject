using UnityEngine;

public class CicloDiaNoite : MonoBehaviour
{
    [Header("Configuração do Ciclo")]
    public float duracaoDoDiaEmSegundos = 120f;

    [Header("Referência da Luz Principal")]
    public Light luzSol;

    [Header("Materiais do Skybox")]
    public Material skyboxDia;
    public Material skyboxNoite;

    [Header("Cores da Luz")]
    public Color corLuzDia = new Color(1f, 0.95f, 0.8f);
    public Color corLuzNoite = new Color(0.05f, 0.05f, 0.2f);

    private bool ehDia = true;

    void Update()
    {
        float velocidadeRotacao = (360f / duracaoDoDiaEmSegundos) * Time.deltaTime;
        transform.Rotate(Vector3.right * velocidadeRotacao);

        float anguloX = transform.eulerAngles.x;

        bool estaDeDia = anguloX > 0f && anguloX < 180f;

        if (estaDeDia != ehDia)
        {
            ehDia = estaDeDia;
            AlternarSkybox(ehDia);
        }

        if (luzSol != null)
        {
            Color corAlvo = ehDia ? corLuzDia : corLuzNoite;
            luzSol.color = Color.Lerp(luzSol.color, corAlvo, Time.deltaTime);
            luzSol.intensity = ehDia ? 1f : 0.1f;
        }
    }

    void AlternarSkybox(bool dia)
    {
        if (dia && skyboxDia != null)
        {
            RenderSettings.skybox = skyboxDia;
        }
        else if (!dia && skyboxNoite != null)
        {
            RenderSettings.skybox = skyboxNoite;
        }

        DynamicGI.UpdateEnvironment();
    }
}