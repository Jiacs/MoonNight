using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PostEffectsManager : MonoBehaviour
{
    [Header("Bloom")]
    [SerializeField] private bool enableBloom;
    [Range(0, 10)]
    public float Intensity = 10;
    [Range(0, 1)]
    public float Threshold = 0.5f;
    [Header("RadiaBlur")]
    [SerializeField] private bool enableRadiaBlur;
    [Range(1, 100)]
    public float Level = 10;
    [Range(0, 1)]
    public float BufferRadius = 0.5f;
    [Range(0, 1)]
    public float CenterX = 0.5f;
    [Range(0, 1)]
    public float CenterY = 0.5f;

    private void Awake()
    {
        if(enableBloom)
        {
            Camera.main.AddComponent<Bloom>();
            Camera.main.GetComponent<Bloom>().Intensity = Intensity;
            Camera.main.GetComponent<Bloom>().Threshold = Threshold;
        }
        if(enableRadiaBlur)
        {
            Camera.main.AddComponent<RadiaBlur>();
            Camera.main.GetComponent<RadiaBlur>().Level = Level;
            Camera.main.GetComponent<RadiaBlur>().BufferRadius = BufferRadius;
            Camera.main.GetComponent<RadiaBlur>().CenterX = CenterX;
            Camera.main.GetComponent<RadiaBlur>().CenterY = CenterY;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
