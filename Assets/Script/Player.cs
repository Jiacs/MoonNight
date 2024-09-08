using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject obj;
    private int currentBullet = 6;
    private int currentHP = 5;
    private GameObject shotVFX;
    private AudioClip shotSE;
    private void Start()
    {
        shotVFX = Resources.Load<GameObject>("VFX/ShotEffect");
        shotSE = Resources.Load<AudioClip>("Audio/Shot");
    }
    void Update()
    {
        if(!GameManager.GameStarted)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Reload();
        }
    }
    private void Shot()
    {
        Vector3 mousePos = Input.mousePosition;
        if (currentBullet <= 0)
        {
            return;
        }
        currentBullet--;
        UIManager.Instance.UpdateBulletUI(currentBullet);
        ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            obj = hit.collider.gameObject;
            if (obj.tag == "Enemy")
            {
                obj.GetComponent<Enemy>().GetHit(20);
            }
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 2.54f));
        pos.z = 3f;
        Instantiate(shotVFX, pos, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shotSE, transform.position);
    }
    private void Reload()
    {
        currentBullet = 6;
        UIManager.Instance.UpdateBulletUI(currentBullet);
    }
    public void GetHit()
    {
        //Instantiate(HitParticle, canvas.transform);
        Debug.Log("Player Get Hit!");
        currentHP--;
        if(currentHP == 0)
        {
            Dead();
        }
        UIManager.Instance.UpdateHPUI(currentHP);
    }
    public void Dead()
    {
        Debug.Log("Game Over");
    }
}
