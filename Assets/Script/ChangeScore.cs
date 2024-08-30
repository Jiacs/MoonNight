using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScore : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private GameObject floatText;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            gameManager.ChangeScore(score);
            GameObject f = Instantiate(floatText, this.gameObject.transform.position, Quaternion.Euler(-1f*Camera.main.transform.rotation.eulerAngles));
            f.GetComponentInChildren<FloatText>().AssignScore(score);
        }
    }
}
