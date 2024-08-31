using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : MonoBehaviour
{
    private Material bloomMaterial;
    [Range(0, 10)]
    public float Intensity = 1;
    [Range(0, 10)]
    public float Threshold = 1;

    void Awake()
    {
        bloomMaterial = new Material(Resources.Load<Shader>("Shader/Bloom"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture RT1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, source.format);
        RenderTexture RT2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
        RenderTexture RT3 = RenderTexture.GetTemporary(source.width / 8, source.height / 8, 0, source.format);
        RenderTexture RT4 = RenderTexture.GetTemporary(source.width / 16, source.height / 16, 0, source.format);
        RenderTexture RT5 = RenderTexture.GetTemporary(source.width / 32, source.height / 32, 0, source.format);
        RenderTexture RT6 = RenderTexture.GetTemporary(source.width / 64, source.height / 64, 0, source.format);
        RenderTexture RT5_up = RenderTexture.GetTemporary(source.width / 32, source.height / 32, 0, source.format);
        RenderTexture RT4_up = RenderTexture.GetTemporary(source.width / 16, source.height / 16, 0, source.format);
        RenderTexture RT3_up = RenderTexture.GetTemporary(source.width / 8, source.height / 8, 0, source.format);
        RenderTexture RT2_up = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
        RenderTexture RT1_up = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, source.format);
        RenderTexture[] rt_list = new RenderTexture[] { RT1 , RT2, RT3, RT4, RT5, RT6,
        RT5_up,RT4_up,RT3_up,RT2_up,RT1_up};
        float intensity = Mathf.Exp(Intensity / 10.0f * 0.693f) - 1.0f;
        bloomMaterial.SetFloat("_Intensity", intensity);
        bloomMaterial.SetFloat("_Threshold", Threshold);
        Graphics.Blit(source, RT1, bloomMaterial, 0);
        Graphics.Blit(RT1, RT2, bloomMaterial, 1);
        Graphics.Blit(RT2, RT3, bloomMaterial, 1);
        Graphics.Blit(RT3, RT4, bloomMaterial, 1);
        Graphics.Blit(RT4, RT5, bloomMaterial, 1);
        Graphics.Blit(RT5, RT6, bloomMaterial, 1);
        bloomMaterial.SetTexture("_BloomTex", RT5);
        Graphics.Blit(RT6, RT5_up, bloomMaterial, 2);
        bloomMaterial.SetTexture("_BloomTex", RT4);
        Graphics.Blit(RT5_up, RT4_up, bloomMaterial, 2);
        bloomMaterial.SetTexture("_BloomTex", RT3);
        Graphics.Blit(RT4_up, RT3_up, bloomMaterial, 2);
        bloomMaterial.SetTexture("_BloomTex", RT2);
        Graphics.Blit(RT3_up, RT2_up, bloomMaterial, 2);
        bloomMaterial.SetTexture("_BloomTex", RT1);
        Graphics.Blit(RT2_up, RT1_up, bloomMaterial, 2);
        bloomMaterial.SetTexture("_BloomTex", RT1_up);
        Graphics.Blit(source, destination, bloomMaterial, 3);

        //release
        for (int i = 0; i < rt_list.Length; i++)
        {
            RenderTexture.ReleaseTemporary(rt_list[i]);
        }
    }
}

