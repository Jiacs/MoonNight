using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField] private GameObject canvas;
    //public GameObject HitParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetHit()
    {
        //Instantiate(HitParticle, canvas.transform);
        Debug.Log("Player Get Hit!");
    }
}
