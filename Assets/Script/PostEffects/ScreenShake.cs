using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Material ScreenShakeMaterial;
    public float shakeFrequency = 0;
    public float shakeAmount = 0;

    void Awake()
    {
        ScreenShakeMaterial = new Material(Resources.Load<Shader>("Shader/ScreenShake"));
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (ScreenShakeMaterial != null)
        {
            ScreenShakeMaterial.SetFloat("_ShakeFrequency", shakeFrequency);
            ScreenShakeMaterial.SetFloat("_ShakeAmount", shakeAmount);
            Graphics.Blit(src, dest, ScreenShakeMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}