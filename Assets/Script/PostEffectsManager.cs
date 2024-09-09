using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PostEffectsManager : MonoBehaviour
{
    [Header("Bloom - Initialize")]
    [SerializeField] private bool bloom_Enable;
    [Range(0, 10)]
    public float bloom_Intensity = 10;
    [Range(0, 1)]
    public float bloom_Threshold = 0.5f;
    private Bloom bloomScript;
    [Header("RadiaBlur - Initialize")]
    [SerializeField] private bool radiaBlur_Enable;
    [Range(1, 100)]
    public float radiaBlur_Level = 10;
    [Range(0, 1)]
    public float radiaBlur_BufferRadius = 0.5f;
    [Range(0, 1)]
    public float radiaBlur_CenterX = 0.5f;
    [Range(0, 1)]
    public float radiaBlur_CenterY = 0.5f;
    private RadiaBlur radialBlurScript;
    [Header("ColorTint - Initialize")]
    [SerializeField] private bool colorTint_Enable;
    public Color colorTint_Color = Color.white;
    private ColorTint colorTintScript;


    #region Singleton
    private static PostEffectsManager instance;
    public static PostEffectsManager Instance => instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log(this.gameObject.name);
            Debug.LogError("More Than One Instance of Singleton!");
        }
    }
    #endregion
    private void Start()
    {
        if (bloom_Enable)
        {
            bloomScript = Camera.main.AddComponent<Bloom>();
        }
        if (radiaBlur_Enable)
        {
            radialBlurScript = Camera.main.AddComponent<RadiaBlur>();
        }
        if(colorTint_Enable)
        {
            colorTintScript = Camera.main.AddComponent<ColorTint>();
        }
        InitializePostEffects();
    }
    public void InitializePostEffects()
    {
        SetUpBloom(bloom_Enable, bloom_Intensity, bloom_Threshold);
        SetUpRadiaBlur(radiaBlur_Enable, radiaBlur_Level, radiaBlur_BufferRadius, radiaBlur_CenterY, radiaBlur_CenterX);
        SetUpColorTint(colorTint_Enable, colorTint_Color);
    }
    public void SetUpBloom(bool enable, float intensity, float threshold)
    {
        if (!bloom_Enable)
        {
            Debug.Log("Bloom is not enabled");
            return;
        }
        bloomScript.enabled = enable;
        bloomScript.Intensity = intensity; 
        bloomScript.Threshold = threshold;
    }
    public void SetUpRadiaBlur(bool enable, float level, float bufferRadius, float centerX, float centerY)
    {
        if (!radiaBlur_Enable)
        {
            Debug.Log("Radial Blur is not enabled");
            return;
        }
        radialBlurScript.enabled = enable;
        radialBlurScript.Level = level;
        radialBlurScript.BufferRadius = bufferRadius;
        radialBlurScript.CenterX = centerX;
        radialBlurScript.CenterY = centerY;
    }
    public void SetUpColorTint(bool enable, Color color)
    {
        if (!colorTint_Enable)
        {
            Debug.Log("Color Tint is not enabled");
            return;
        }
        colorTintScript.enabled = enable;
        colorTintScript.TintColor = color;
    }
    public void RemoveRadialBlur(float speed = 1)
    {
        //remove blur effect dynamically
        if (radiaBlur_Enable)
        {
            if (radialBlurScript.Level > 1)
            {
                radialBlurScript.Level -= speed;
            }
        }
    }
    public IEnumerator GradientTintColor(Color color, float time)
    {
        float offset_r = (color.r - colorTintScript.TintColor.r)/time;
        float offset_g = (color.g - colorTintScript.TintColor.g)/ time;
        float offset_b = (color.b - colorTintScript.TintColor.b)/ time;
        float timer = 0;
        while (timer <= time)
        {
            colorTintScript.TintColor.r += offset_r;
            colorTintScript.TintColor.g += offset_g;
            colorTintScript.TintColor.b += offset_b;
            yield return null;
        }
        colorTintScript.TintColor = color;
    }
}
