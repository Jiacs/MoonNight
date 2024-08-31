using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiaBlur : MonoBehaviour
{

    private Material RadiaBlurMaterial;
    [Range(1, 100)]
    public float Level = 10;
    [Range(0, 1)]
    public float BufferRadius = 0.5f;
    [Range(0, 1)]
    public float CenterX = 0.5f;
    [Range(0, 1)]
    public float CenterY = 0.5f;

    void Awake()
    {
        RadiaBlurMaterial = new Material(Resources.Load<Shader>("Shader/RadiaBlur"));
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (RadiaBlurMaterial != null)
        {
            RadiaBlurMaterial.SetFloat("_Level", Level);
            RadiaBlurMaterial.SetFloat("_CenterX", CenterX);
            RadiaBlurMaterial.SetFloat("_CenterY", CenterY);
            RadiaBlurMaterial.SetFloat("_BufferRadius", BufferRadius);

            Graphics.Blit(src, dest, RadiaBlurMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
    public void BlurEffect(float level, float speed = 30)
    {
        StartCoroutine(BlurEffectCoroutine(level, speed));
    }
    private IEnumerator BlurEffectCoroutine(float level, float speed)
    {
        while(Level < level)
        {
            Level += Time.deltaTime * speed;
            yield return null;
        }
        Level = level;
        while(Level > 1)
        {
            Level -= Time.deltaTime * speed * 2;
            yield return null;
        }
        Level = 1;
    }
}