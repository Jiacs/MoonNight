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
    public void UpdateHPUI(int currentHP)
    {
        //TODO: This function will be called when player's HP changes.
        //follow the example of UpdateBulletUI to Update HP UI.
    }
    public void UpdateBulletUI(int currentBullet)
    {
        int count = BulletUI.transform.childCount;
        if(currentBullet > count)
        {
            Debug.LogError("Invalid bullet amount, greater than capacity");
            return;
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

    private void Start()
    {
        initializeUIValues();
        //Bind Listener for bloom UI
        bloom_Toggle.onValueChanged.AddListener(delegate { UpdateBloom(); });
        bloom_IntesitySlider.onValueChanged.AddListener(delegate { UpdateBloom(); });
        bloom_ThresholdSlider.onValueChanged.AddListener(delegate { UpdateBloom(); });
        //TODO Bind Listener for rest post effects UI 
    }
    private void initializeUIValues()
    {
        bloom_IntesitySlider.value = PostEffectsManager.Instance.bloom_Intensity;
        bloom_ThresholdSlider.value = PostEffectsManager.Instance.bloom_Threshold;
        radialBlur_LevelSlider.value = PostEffectsManager.Instance.radiaBlur_Level;
        radialBlur_BufferRadiusSlider.value = PostEffectsManager.Instance.radiaBlur_BufferRadius;
        radialBlur_CenterXSlider.value = PostEffectsManager.Instance.radiaBlur_CenterX;
        radialBlur_CenterYSlider.value = PostEffectsManager.Instance.radiaBlur_CenterY;
    }
    public void UpdateBloom()
    {
        PostEffectsManager.Instance.SetUpBloom(bloom_Toggle.isOn, bloom_IntesitySlider.value, bloom_ThresholdSlider.value);
    }
    //TODO Create Update function for rest post effects UI
    #endregion
}
