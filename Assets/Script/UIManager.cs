using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance;
    public static UIManager Instance => instance;
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

    #region GameUI
    [SerializeField] private GameObject HPUI;
    [SerializeField] private GameObject BulletUI;
    [SerializeField] private GameObject BulletPre;
    [SerializeField] private GameObject HPPre;
    public void UpdateHPUI(int currentHP)
    {
        int count = HPUI.transform.childCount;
        if(currentHP > count)
        {
            Instantiate(HPPre, HPUI.transform);
            UpdateHPUI(currentHP);
        }
        for(int i = 0; i < count; i++)
        {
            if (i < currentHP)
            {
                HPUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HPUI.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void UpdateBulletUI(int currentBullet)
    {
        int count = BulletUI.transform.childCount;
        if(currentBullet > count)
        {
            Instantiate(BulletPre, BulletUI.transform);
            UpdateBulletUI(currentBullet);
        }
        for(int i = 0; i < count; i++)
        {
            if (i < currentBullet)
            {
                BulletUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                BulletUI.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region PostEffects UI
    [Header("Bloom UI")]
    [SerializeField] private Toggle bloom_Toggle;
    [SerializeField] private Slider bloom_IntesitySlider;
    [SerializeField] private Slider bloom_ThresholdSlider;
    [Header("Radial Blur UI")]
    [SerializeField] private Toggle radialBlur_Toggle;
    [SerializeField] private Slider radialBlur_LevelSlider;
    [SerializeField] private Slider radialBlur_BufferRadiusSlider;
    [SerializeField] private Slider radialBlur_CenterXSlider;
    [SerializeField] private Slider radialBlur_CenterYSlider;
    [Header("Color Tint UI")]
    [SerializeField] private Toggle colorTint_Toggle;
    [SerializeField] private Slider colorTint_RSlider;
    [SerializeField] private Slider colorTint_GSlider;
    [SerializeField] private Slider colorTint_BSlider;
    [SerializeField] private Image colorTint_SampleColor;
    [Header("Image Blend UI")]
    [SerializeField] private Toggle imageBlend_Toggle;
    [SerializeField] private Slider imageBlend_AlphaSlider;
    [SerializeField] private Slider imageBlend_CenterXSlider;
    [SerializeField] private Slider imageBlend_CenterYSlider;
    [SerializeField] private Slider imageBlend_ScaleXSlider;
    [SerializeField] private Slider imageBlend_ScaleYSlider;
    [Header("Screen Shake UI")]
    [SerializeField] private Toggle screenShake_Toggle;
    [SerializeField] private Slider screenShake_FrequencySlider;
    [SerializeField] private Slider screenShake_AmountSlider;
    [Header("Desaturate UI")]
    [SerializeField] private Toggle desaturate_Toggle;
    [SerializeField] private Slider desaturate_AmountSlider;
    [Header("Screen Broken UI")]
    [SerializeField] private Toggle screenBroken_Toggle;
    [SerializeField] private Slider screenBroken_NormalScaleSlider;

    private Texture2D imageBlend_Texture;
    private Texture2D screenBroken_NormalTexture;

    private void Start()
    {
        initializeUIValues();
        //Bind Listener for bloom UI
        bloom_Toggle.onValueChanged.AddListener(delegate { UpdateBloom(); });
        bloom_IntesitySlider.onValueChanged.AddListener(delegate { UpdateBloom(); });
        bloom_ThresholdSlider.onValueChanged.AddListener(delegate { UpdateBloom(); });

        radialBlur_Toggle.onValueChanged.AddListener(delegate {updateRadialBlur(); });
        radialBlur_LevelSlider.onValueChanged.AddListener(delegate {updateRadialBlur(); });
        radialBlur_BufferRadiusSlider.onValueChanged.AddListener(delegate {updateRadialBlur(); });
        radialBlur_CenterXSlider.onValueChanged.AddListener(delegate {updateRadialBlur(); });
        radialBlur_CenterYSlider.onValueChanged.AddListener(delegate {updateRadialBlur(); });
        //Bind Listener for color Tint
        colorTint_Toggle.onValueChanged.AddListener(delegate { UpdateColorTint(); });
        colorTint_RSlider.onValueChanged.AddListener(delegate { UpdateColorTint(); });
        colorTint_GSlider.onValueChanged.AddListener(delegate { UpdateColorTint(); });
        colorTint_BSlider.onValueChanged.AddListener(delegate { UpdateColorTint(); });

        imageBlend_AlphaSlider.onValueChanged.AddListener(delegate { UpdateImageBlend(); });
        imageBlend_Toggle.onValueChanged.AddListener(delegate { UpdateImageBlend(); });
        imageBlend_CenterXSlider.onValueChanged.AddListener (delegate { UpdateImageBlend(); });
        imageBlend_CenterYSlider.onValueChanged.AddListener(delegate { UpdateImageBlend(); });
        imageBlend_ScaleXSlider.onValueChanged.AddListener(delegate { UpdateImageBlend(); });
        imageBlend_ScaleYSlider.onValueChanged.AddListener(delegate { UpdateImageBlend(); });

        screenShake_Toggle.onValueChanged.AddListener(delegate { UpdateScreenShake(); });
        screenShake_FrequencySlider.onValueChanged.AddListener(delegate { UpdateScreenShake(); });
        screenShake_AmountSlider.onValueChanged.AddListener(delegate { UpdateScreenShake(); });

        desaturate_Toggle.onValueChanged.AddListener(delegate { UpdateDesaturate(); });
        desaturate_AmountSlider.onValueChanged.AddListener(delegate { UpdateDesaturate(); });

        screenBroken_Toggle.onValueChanged.AddListener(delegate { UpdateScreenBroken(); });
        screenBroken_NormalScaleSlider.onValueChanged.AddListener(delegate { UpdateScreenBroken(); });
    }
    private void initializeUIValues()
    {
        bloom_IntesitySlider.value = PostEffectsManager.Instance.bloom_Intensity;
        bloom_ThresholdSlider.value = PostEffectsManager.Instance.bloom_Threshold;
        radialBlur_LevelSlider.value = PostEffectsManager.Instance.radiaBlur_Level;
        radialBlur_BufferRadiusSlider.value = PostEffectsManager.Instance.radiaBlur_BufferRadius;
        radialBlur_CenterXSlider.value = PostEffectsManager.Instance.radiaBlur_CenterX;
        radialBlur_CenterYSlider.value = PostEffectsManager.Instance.radiaBlur_CenterY;
        colorTint_RSlider.value = PostEffectsManager.Instance.colorTint_Color.r * 255;
        colorTint_GSlider.value = PostEffectsManager.Instance.colorTint_Color.g * 255;
        colorTint_BSlider.value = PostEffectsManager.Instance.colorTint_Color.b * 255;
        imageBlend_AlphaSlider.value = PostEffectsManager.Instance.imageBlend_Alpha;
        imageBlend_Texture = PostEffectsManager.Instance.imageBlend_Texture;
        imageBlend_CenterXSlider.value = PostEffectsManager.Instance.imageBlend_ImagePos.x;
        imageBlend_CenterYSlider.value = PostEffectsManager.Instance.imageBlend_ImagePos.y;
        imageBlend_ScaleXSlider.value = PostEffectsManager.Instance.imageBlend_ImageScale.x;
        imageBlend_ScaleYSlider.value = PostEffectsManager.Instance.imageBlend_ImageScale.y;
        screenShake_FrequencySlider.value = PostEffectsManager.Instance.shakeFrequency;
        screenShake_AmountSlider.value = PostEffectsManager.Instance.shakeAmount;
        desaturate_AmountSlider.value = PostEffectsManager.Instance.desaturateAmount;
        screenBroken_NormalTexture = PostEffectsManager.Instance.screenBrokenNormalTexture;
        screenBroken_NormalScaleSlider.value = PostEffectsManager.Instance.screenBrokenNormalScale;
    }
    public void UpdateBloom()
    {
        PostEffectsManager.Instance.SetUpBloom(bloom_Toggle.isOn, bloom_IntesitySlider.value, bloom_ThresholdSlider.value);
    }
    //TODO Create Update function for rest post effects UI

    private void updateRadialBlur()
    {
        PostEffectsManager.Instance.SetUpRadiaBlur(
            radialBlur_Toggle.isOn, 
            radialBlur_LevelSlider.value, 
            radialBlur_BufferRadiusSlider.value,
            radialBlur_CenterXSlider.value,
            radialBlur_CenterYSlider.value
        );
    }

    public void UpdateColorTint()
    {
        Color color = new Color(colorTint_RSlider.value / 255f, colorTint_GSlider.value / 255f, colorTint_BSlider.value / 255f);
        PostEffectsManager.Instance.SetUpColorTint(colorTint_Toggle.isOn, color);
        colorTint_SampleColor.color = color;
    }

    public void UpdateImageBlend()
    {
        imageBlend_Texture = Camera.main.GetComponent<ImageBlend>().imageTexture;
        /*
        imageBlend_Image.sprite = Sprite.Create(
            imageBlend_Texture,
            new Rect(0, 0, imageBlend_Texture.width, imageBlend_Texture.height),
            new Vector2(0.5f, 0.5f)
        );
        */
        PostEffectsManager.Instance.SetUpImageBlend(imageBlend_Toggle.isOn, imageBlend_Texture, imageBlend_AlphaSlider.value,
            new Vector2(imageBlend_CenterXSlider.value, imageBlend_CenterYSlider.value), 
            new Vector2(imageBlend_ScaleXSlider.value, imageBlend_ScaleYSlider.value));
    }
    public void UpdateScreenShake()
    {
        PostEffectsManager.Instance.SetUpScreenShake(screenShake_Toggle.isOn, screenShake_FrequencySlider.value, screenShake_AmountSlider.value, true);
    }
    public void UpdateDesaturate()
    {
        PostEffectsManager.Instance.SetUpDesaturate(desaturate_Toggle.isOn, desaturate_AmountSlider.value);
    }
    public void UpdateScreenBroken()
    {
        PostEffectsManager.Instance.SetUpScreenBroken(screenBroken_Toggle.isOn, screenBroken_NormalScaleSlider.value, screenBroken_NormalTexture);
    }
    #endregion
}
