using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject obj;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }
    }
    private void Shot()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            obj = hit.collider.gameObject;
            if (obj.tag == "Enemy")
            {
                obj.GetComponent<Enemy>().GetHit(20);
            }
        }
    }
    public void GetHit()
    {
        //Instantiate(HitParticle, canvas.transform);
        Debug.Log("Player Get Hit!");
    }
}
