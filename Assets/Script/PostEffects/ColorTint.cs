using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTint : MonoBehaviour
{

    private Material ColorTintMaterial;
    public Color TintColor;

    void Awake()
    {
        ColorTintMaterial = new Material(Resources.Load<Shader>("Shader/ColorTint"));
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (ColorTintMaterial != null)
        {
            ColorTintMaterial.SetColor("_Color", TintColor);

            Graphics.Blit(src, dest, ColorTintMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}