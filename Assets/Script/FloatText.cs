using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
    public void AssignScore(int s)
    {
        if (s >= 0)
        {
            text.text = "+" + s.ToString();
        }
        else
        {
            text.text = s.ToString();
        }
    }
}
