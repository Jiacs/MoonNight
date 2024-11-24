using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    public UnityEngine.Color colorTint_Color = UnityEngine.Color.white;
    private ColorTint colorTintScript;
    [Header("ImageBlend - Initialize")]
    [SerializeField] private bool imageBlend_Enable;
    public Texture2D imageBlend_Texture;
    [Range(0, 1)]
    public float imageBlend_Alpha = 0.5f;
    private ImageBlend imageBlendScript;
    public Vector2 imageBlend_ImagePos;
    public Vector2 imageBlend_ImageScale;
    [Header("ScreenShake - Initialize")]
    [SerializeField] private bool screenShake_Enable;
    private ScreenShake screenShakeScript;
    public float shakeFrequency = 0;
    public float shakeAmount = 0;
    [Header("Desaturate - Initialize")]
    [SerializeField] private bool desaturate_Enable;
    private Desaturate desaturateScript;
    public float desaturateAmount = 0;


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
        if(imageBlend_Enable)
        {
            imageBlendScript = Camera.main.AddComponent<ImageBlend>();
        }
        if(screenShake_Enable)
        {
            screenShakeScript = Camera.main.AddComponent<ScreenShake>();
        }
        if(desaturate_Enable)
        {
            desaturateScript = Camera.main.AddComponent<Desaturate>();
        }
        InitializePostEffects();
    }
    public void InitializePostEffects()
    {
        SetUpBloom(bloom_Enable, bloom_Intensity, bloom_Threshold);
        SetUpRadiaBlur(radiaBlur_Enable, radiaBlur_Level, radiaBlur_BufferRadius, radiaBlur_CenterY, radiaBlur_CenterX);
        SetUpColorTint(colorTint_Enable, colorTint_Color);
        SetUpImageBlend(imageBlend_Enable, imageBlend_Texture, imageBlend_Alpha, imageBlend_ImagePos, imageBlend_ImageScale);
        SetUpDesaturate(desaturate_Enable, desaturateAmount);
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
    public void SetUpColorTint(bool enable, UnityEngine.Color color)
    {
        if (!colorTint_Enable)
        {
            Debug.Log("Color Tint is not enabled");
            return;
        }
        colorTintScript.enabled = enable;
        colorTintScript.TintColor = color;
    }
    public void SetUpImageBlend(bool enable, Texture2D imageTexture, float alpha, Vector2 imagePos, Vector2 imageScale)
    {
        if(!imageBlend_Enable)
        {
            Debug.Log("Blend Image is not enabled");
            return;
        }

        imageBlendScript.enabled = enable;
        imageBlendScript.imageTexture = imageTexture;
        imageBlendScript.alpha = alpha;
        imageBlendScript.imagePos = imagePos;
        imageBlendScript.imageScale = imageScale;
    }
    public void SetUpScreenShake(bool enable, float shakeFrequency, float shakeAmount)
    {
        if (!screenShake_Enable)
        {
            Debug.Log("Screen Shake is not enabled");
            return;
        }
        screenShakeScript.enabled = enable;
        if(shakeAmount < screenShakeScript.shakeAmount)
        {
            return;
        }
        screenShakeScript.shakeFrequency = shakeFrequency;
        screenShakeScript.shakeAmount = shakeAmount;
    }
    public void SetUpDesaturate(bool enable, float desaturateAmount)
    {
        if (!desaturate_Enable)
        {
            Debug.Log("Desaturate is not enabled");
            return;
        }
        desaturateScript.enabled = enable;
        desaturateScript.desaturateAmount = desaturateAmount;
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
    public IEnumerator GradientTintColor(UnityEngine.Color color, float time)
    {
        float offset_r = (color.r - colorTintScript.TintColor.r)/time;
        float offset_g = (color.g - colorTintScript.TintColor.g)/ time;
        float offset_b = (color.b - colorTintScript.TintColor.b)/ time;
        float timer = 0;
        while (timer <= time)
        {
            colorTintScript.TintColor.r += offset_r * Time.deltaTime;
            colorTintScript.TintColor.g += offset_g * Time.deltaTime; ;
            colorTintScript.TintColor.b += offset_b * Time.deltaTime; ;
            timer += Time.deltaTime;
            yield return null;
        }
        colorTintScript.TintColor = color;
    }
    public IEnumerator ShakeScreen(float shakeAmount, float shakeFreq, float time)
    {
        float timer = 0;
        float amount_off = (shakeAmount - screenShakeScript.shakeAmount) / time;
        float freq_off = (shakeFreq - screenShakeScript.shakeFrequency) / time;
        while(timer < time)
        {
            screenShakeScript.shakeAmount += amount_off * Time.deltaTime;
            screenShakeScript.shakeFrequency += freq_off * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        screenShakeScript.shakeAmount = shakeAmount;
        screenShakeScript.shakeFrequency = shakeFreq;
    }
    public IEnumerator DesaturateScreen(float desaturateAmount, float time)
    {
        float offset= (desaturateAmount - desaturateScript.desaturateAmount) / time;
        float timer = 0;
        while (timer <= time)
        {
            desaturateScript.desaturateAmount += offset * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        desaturateScript.desaturateAmount = desaturateAmount;
    }
    public IEnumerator BloomThresholdCoroutine(float threshold, float time)
    {
        float offset = (threshold - bloomScript.Threshold) / time;
        float timer = 0;
        while (timer <= time)
        {
            bloomScript.Threshold += offset * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        bloomScript.Threshold = threshold;
    }
}
