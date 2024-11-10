using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject obj;
    private int currentBullet = 6;
    private GameObject shotVFX;
    private AudioClip shotSE;
    public static bool isDead = false;

    private int bulletCap = 6;
    private int damage = 20;
    private int maxHP = 5;
    private int currentHP = 5;

    private void Start()
    {
        shotVFX = Resources.Load<GameObject>("VFX/ShotEffect");
        shotSE = Resources.Load<AudioClip>("Audio/Shot");
    }
    void Update()
    {
        if(!GameManager.GameStarted || currentHP == 0)
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
            obj = hit.collider.gameObject;
            if (obj.tag == "Enemy")
            {
                obj.GetComponent<Enemy>().GetHit(damage);
            }
            else if (obj.tag == "EnemyHead")
            {
                obj.GetComponentInParent<Enemy>().GetHit(3*damage);
            }
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 2.54f));
        pos.z = 3f;
        Instantiate(shotVFX, pos, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shotSE, transform.position);
        //Add radial blur effects
        PostEffectsManager.Instance.SetUpRadiaBlur(true, 10, 1, mousePos.x / Screen.width, mousePos.y / Screen.height);
    }
    private void Reload()
    {
        currentBullet = bulletCap;
        UIManager.Instance.UpdateBulletUI(currentBullet);
        //TODO: Add a sound effect here, collect a reload sound effect online,
        //drag it into Resources/Audio folder. Then follow the example of add 
        //SE for gun shot to add it.
    }
    public void GetHit()
    {
        //Instantiate(HitParticle, canvas.transform);
        if(currentHP <= 0)
        {
            return;
        }
        Debug.Log("Player Get Hit!");
        currentHP--;
        UIManager.Instance.UpdateHPUI(currentHP);
        if (currentHP == 0)
        {
            Dead();
            return;
        }
        StartCoroutine(PostEffectsManager.Instance.GradientTintColor(new Color(1, 0.2f * currentHP, 0.2f * currentHP), 1));
    }
    public void Dead()
    {
        StartCoroutine(PostEffectsManager.Instance.GradientTintColor(Color.black, 3));
        isDead = true;
        Debug.Log("Game Over");
    }
    public void Enhance(Buff buff)
    {
        switch (buff)
        {
            case Buff.BulletCapIncrease:
                bulletCap++;
                break;
            case Buff.HPCountIncrease:
                maxHP++;
                currentHP++;
                UIManager.Instance.UpdateHPUI(currentHP);
                break;
            case Buff.DamageIncrease:
                damage = (int)(1.5 * damage);
                break;
            case Buff.RecoverAllHP:
                currentHP = maxHP;
                UIManager.Instance.UpdateHPUI(currentHP);
                break;
        }
    }
}

public enum Buff
{
    BulletCapIncrease,
    HPCountIncrease,
    DamageIncrease,
    RecoverAllHP
}