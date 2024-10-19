using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBlend : MonoBehaviour
{

    private Material ImageBlendMaterial;

    public Texture2D imageTexture;

    [Range(0, 1)]
    public float alpha = 1.0f;

    void Awake()
    {
        ImageBlendMaterial = new Material(Resources.Load<Shader>("Shader/ImageBlend"));
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (ImageBlendMaterial != null)
        {
            ImageBlendMaterial.SetTexture("_ImageTex", imageTexture);
            ImageBlendMaterial.SetFloat("_Alpha", alpha);

            Graphics.Blit(src, dest, ImageBlendMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}