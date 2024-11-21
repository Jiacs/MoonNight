using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakPoints : MonoBehaviour
{
    Boss boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion tempRot = this.transform.rotation;
        tempRot.eulerAngles = new Vector3(90, 0, 0);
        transform.rotation = tempRot;
    }
    public void GetHit(int damage)
    {
        boss.EnabledWeakPoints--;
        this.gameObject.SetActive(false);
        boss.GetHit(damage);
    }
}
