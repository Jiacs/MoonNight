using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desaturate : MonoBehaviour
{
    private Material DesaturateMaterial;
    [Range(0, 1)]
    public float desaturateAmount = 0.0f;

    void Awake()
    {
        DesaturateMaterial = new Material(Resources.Load<Shader>("Shader/Desaturate"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (DesaturateMaterial != null)
        {
            DesaturateMaterial.SetFloat("_Desaturation", desaturateAmount);
            Graphics.Blit(src, dest, DesaturateMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
