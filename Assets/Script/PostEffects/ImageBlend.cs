using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBlend : MonoBehaviour
{
    private Material ImageBlendMaterial;
    public Texture2D imageTexture;
    [Range(0, 1)]
    public float alpha = 1.0f;
    public Vector2 imagePos = new Vector2(0.5f, 0.5f);
    public Vector2 imageScale = new Vector2(1.0f, 1.0f);

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
            ImageBlendMaterial.SetVector("_ImagePos", imagePos);
            ImageBlendMaterial.SetVector("_ImageScale", imageScale);

            Graphics.Blit(src, dest, ImageBlendMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
