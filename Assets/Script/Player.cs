using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Vector3 Position;
    private Ray ray;
    private RaycastHit hit;
    private GameObject obj;
    private int currentBullet = 6;
    private GameObject shotVFX;
    private AudioClip shotSE;
    private AudioClip reloadSE;
    private AudioClip hitSE;
    public static bool isDead = false;

    private int bulletCap = 6;
    private int damage = 20;
    private int maxHP = 5;
    private int currentHP = 5;
    private bool autoReload = false;
    private bool autoShot = false;
    private float reloadInterval = 1;
    private float autoShotTimer = 0;
    private float shotInterval = 0.3f;
    private float timeSlowTime = 0.5f;
    private float timeSlowTimer;
    private float timeSlowMult = 0.2f;
    private bool inTimeSlow = false;
    private Coroutine autoReloadCoroutine;
    private void Awake()
    {
        Position = transform.position;
    }
    private void Start()
    {
        shotVFX = Resources.Load<GameObject>("VFX/ShotEffect");
        shotSE = Resources.Load<AudioClip>("Audio/Shot");
        hitSE = Resources.Load<AudioClip>("Audio/Hit");
        reloadSE = Resources.Load<AudioClip>("Audio/Reload");
    }
    void Update()
    {
        if(!GameManager.GameStarted || currentHP == 0)
        {
            return;
        }
        timeSlowTimer += Time.deltaTime * 0.5f;
        if(timeSlowTimer >= timeSlowTime)
        {
            timeSlowTimer = timeSlowTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }
        if(Input.GetMouseButton(0) && autoShot)
        {
            //count and auto shot
            autoShotTimer += Time.deltaTime;
            if(autoShotTimer >= shotInterval)
            {
                Shot();
                autoShotTimer = 0;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            autoShotTimer = 0;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (autoReloadCoroutine != null)
            {
                StopCoroutine(autoReloadCoroutine);
            }
            Reload();
            StartTimeSlow();
        }
        if(Input.GetMouseButton(1) && inTimeSlow)
        {
            timeSlowTimer -= Time.deltaTime;
            if(timeSlowTimer <= 0)
            {
                EndTimeSlow();
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            EndTimeSlow();
        }
    }
    private void Shot()
    {
        Vector3 mousePos = Input.mousePosition;
        if (currentBullet <= 0)
        {
            if (autoReload && autoReloadCoroutine == null)
            {
                autoReloadCoroutine = StartCoroutine(AutoReload(reloadInterval));
            }
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
            else if (obj.tag == "OptionBlock")
            {
                obj.GetComponent<OptionBlock>().GetHit();
            }
            else if (obj.tag == "Boss")
            {
                obj.GetComponent<Boss>().BossGetHit(damage * 0.1f);
            }
            else if (obj.tag == "BossWeakPoint")
            {
                obj.GetComponent<EnemyWeakPoints>().GetHit(damage);
            }
            else if (obj.tag == "Axe")
            {
                obj.GetComponent<Axe>().GetHit();
            }
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 2.54f));
        pos.z = 3f;
        Instantiate(shotVFX, pos, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shotSE, transform.position, 0.3f);
        //Add radial blur effects
        PostEffectsManager.Instance.SetUpRadiaBlur(true, 10, 1, mousePos.x / Screen.width, mousePos.y / Screen.height);
        //Add screen shake effects
        PostEffectsManager.Instance.SetUpScreenShake(true, 1, 0.01f);
        StartCoroutine(PostEffectsManager.Instance.ShakeScreen(0, 0, 0.2f));
    }
    private IEnumerator AutoReload(float interval)
    {
        yield return new WaitForSeconds(interval);
        Reload();
    }
    private void Reload()
    {
        currentBullet = bulletCap;
        autoReloadCoroutine = null;
        UIManager.Instance.UpdateBulletUI(currentBullet);
        AudioSource.PlayClipAtPoint(reloadSE, transform.position);
    }
    private void StartTimeSlow()
    {
        if (inTimeSlow)
        {
            return;
        }
        inTimeSlow = true;
        Time.timeScale -= timeSlowMult;
    }
    private void EndTimeSlow()
    {
        if(!inTimeSlow)
        {
            return;
        }
        inTimeSlow = false;
        Time.timeScale += timeSlowMult;
    }
    Coroutine BlendBloodCoroutine;
    public void GetHit()
    {
        //Instantiate(HitParticle, canvas.transform);
        if(currentHP <= 0)
        {
            return;
        }
        Debug.Log("Player Get Hit!");
        AudioSource.PlayClipAtPoint(hitSE, transform.position);
        if(BlendBloodCoroutine != null)
        {
            StopCoroutine(BlendBloodCoroutine);
        }
        BlendBloodCoroutine = StartCoroutine(BlendBloodImage());
        currentHP--;
        UIManager.Instance.UpdateHPUI(currentHP);
        if (currentHP == 0)
        {
            Dead();
            return;
        }
        StartCoroutine(PostEffectsManager.Instance.GradientTintColor(new Color(1, Mathf.Min(1, 0.2f * currentHP), Mathf.Min(1, 0.2f * currentHP)), 1));
        PostEffectsManager.Instance.SetUpScreenShake(true, 1, 0.05f);
        StartCoroutine(PostEffectsManager.Instance.ShakeScreen(0, 0, 0.5f));
        float originalBloomThreshold = PostEffectsManager.Instance.bloom_Threshold;
        float originalBloomIntensity = PostEffectsManager.Instance.bloom_Intensity;
        PostEffectsManager.Instance.SetUpBloom(true, originalBloomIntensity, 0);
        StartCoroutine(PostEffectsManager.Instance.BloomThresholdCoroutine(originalBloomThreshold, 0.3f));
    }
    private IEnumerator BlendBloodImage()
    {
        ImageBlend imageBlendScript = Camera.main.GetComponent<ImageBlend>();
        imageBlendScript.imagePos = new Vector2(UnityEngine.Random.Range(0.1f, 0.3f), UnityEngine.Random.Range(-0.1f, 0.1f));
        while(imageBlendScript.alpha < 1)
        {
            imageBlendScript.alpha += Time.deltaTime * 10;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        while (imageBlendScript.alpha > 0)
        {
            imageBlendScript.alpha -= Time.deltaTime * 3;
            yield return null;
        }
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
                PostEffectsManager.Instance.SetUpColorTint(true, new Color(1, Mathf.Min(1, 0.2f * currentHP), Mathf.Min(1, 0.2f * currentHP)));
                UIManager.Instance.UpdateHPUI(currentHP);
                break;
            case Buff.DamageIncrease:
                damage = (int)(1.1 * damage);
                break;
            case Buff.RecoverAllHP:
                currentHP = maxHP;
                PostEffectsManager.Instance.SetUpColorTint(true, Color.white);
                UIManager.Instance.UpdateHPUI(currentHP);
                break;
            case Buff.AutoReload:
                if (autoReload)
                {
                    reloadInterval *= 0.9f;
                }
                else
                {
                    autoReload = true;
                }
                break;
            case Buff.AutoShot:
                if (autoShot)
                {
                    shotInterval *= 0.9f;
                }
                else
                {
                    autoShot = true;
                }
                break;
            case Buff.TimeSlowTime:
                timeSlowTime += 0.1f;
                break;
            case Buff.TimeSlowMult:
                timeSlowMult += (0.5f - timeSlowMult)* 0.3f;
                break;

        }
    }
}
