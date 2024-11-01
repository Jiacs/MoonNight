using UnityEngine;

public class CombinedShader : MonoBehaviour
{
    [SerializeField] private Material combinedMaterial;
    private RenderTexture tempTexture;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (tempTexture == null || tempTexture.width != src.width || tempTexture.height != src.height)
        {
            if (tempTexture != null)
            {
                tempTexture.Release();
            }
            tempTexture = new RenderTexture(src.width, src.height, 0);
        }

        Graphics.Blit(src, tempTexture, combinedMaterial, 0); 

        Graphics.Blit(tempTexture, dest, combinedMaterial, 1);  
    }

    void OnDestroy()
    {
        if (tempTexture != null)
        {
            tempTexture.Release();
        }
    }
}
