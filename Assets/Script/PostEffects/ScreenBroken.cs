using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenBroken : MonoBehaviour
{

    private Material mat;
    public Texture2D brokenNormalMap;
    public float NormalScale = 0;
    void Awake()
    {
        mat = new Material(Resources.Load<Shader>("Shader/ScreenBroken"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture src0 = RenderTexture.GetTemporary(source.width, source.height);
        mat.SetTexture("_MainTex", source);
        mat.SetTexture("_BrokenNormalMap", brokenNormalMap);
        mat.SetFloat("_BrokenScale", NormalScale);
        Graphics.Blit(source, src0, mat, 0);
        Graphics.Blit(src0, destination);

        RenderTexture.ReleaseTemporary(src0);
    }
}
