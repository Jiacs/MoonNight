using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject[] weakPoints;
    [HideInInspector] public int EnabledWeakPoints = 0;
    [SerializeField] private RectTransform hpBarFill;
    [SerializeField] private RectTransform hpBarDamp;
    [SerializeField] private GameObject axe;
    [SerializeField] private float throwAxesCD;
    [SerializeField] private Transform bossHandTrans;
    private float throwAxesTimer;
    private float dampDecreaseSpeed = 0.05f;
    private int throwedAxes;
    [HideInInspector] public bool readyToThrow = true;
    protected override void Update()
    {
        roarTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        throwAxesTimer += Time.deltaTime;
        FaceToPlayer();

        //HP Bar update
        if (hpBarDamp.localScale.x > hpBarFill.localScale.x)
        {
            Vector3 temp = hpBarDamp.localScale;
            temp.x -= dampDecreaseSpeed * Time.deltaTime;
            if (temp.x < hpBarFill.localScale.x)
            {
                temp.x = hpBarFill.localScale.x;
            }
            hpBarDamp.localScale = temp;
        }

        //Skill throw Axes
        if(throwAxesTimer > throwAxesCD)
        {
            if (throwedAxes > 3)
            {
                throwAxesTimer = 0;
                throwedAxes = 0;
                this.transform.position = new Vector3(-0.2f, 0f, 10f);
                return;
            }
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);
            ThrowAxesSkill();
        }
        else
        {
            //Normal Attack
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) > attackDistance)
            {
                this.transform.forward = player.transform.position - this.transform.position;
                this.GetComponent<Rigidbody>().velocity = this.transform.forward * moveSpeed;
                animator.SetBool("IsWalking", true);
            }
            else
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                animator.SetBool("IsWalking", false);
                Attack();
            }
        }
    }
    private void ThrowAxesSkill()
    {
        if(!readyToThrow)
        {
            return;
        }
        readyToThrow = false;
        throwedAxes++;
        animator.ResetTrigger("Attack");
        this.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), 0, UnityEngine.Random.Range(42f, 16f));
        animator.SetTrigger("ThrowAxe");
    }
    public void ShowWeakPoints()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(PostEffectsManager.Instance.DesaturateScreen(0.8f, 0.1f));
        int count = Mathf.Min(2 + level / 10, 7);
        List<int> numbers = new List<int>();
        for (int i = 0; i < weakPoints.Length; i++)
        {
            numbers.Add(i);
        }

        List<int> randomNumbers = new List<int>();

        while (randomNumbers.Count < count)
        {
            int index = UnityEngine.Random.Range(0, numbers.Count);
            randomNumbers.Add(numbers[index]);
            numbers.RemoveAt(index);
        }
        for(int i = 0; i<randomNumbers.Count; i++)
        {
            weakPoints[randomNumbers[i]].SetActive(true);
        }
        EnabledWeakPoints = count;
    }
    public void RemoveAllWeakPoints()
    {
        GetComponent<Collider>().enabled = true;
        EnabledWeakPoints = 0;
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetActive(false);
        }
    }
    public void BossGetHit(float damage)
    {
        if (isDead)
        {
            return;
        }
        currentHP = Math.Max(currentHP - damage, 0);
        Vector3 temp = hpBarFill.localScale;
        temp.x = (float)currentHP / maxHP;
        hpBarFill.localScale = temp;
        if (currentHP == 0)
        {
            Dead();
        }
    }
    public override void GetHit(float damage)
    {
        if (isDead)
        {
            return;
        }
        if (EnabledWeakPoints == 0)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");
            currentHP -= 3*damage;
            GetComponent<Collider>().enabled = true;
        }
        currentHP = Math.Max(currentHP - damage, 0);
        Vector3 temp = hpBarFill.localScale;
        temp.x = (float)currentHP / maxHP;
        hpBarFill.localScale = temp;
        if (currentHP == 0)
        {
            Dead();
        }

    }
    public void ThrowAxe()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GameObject Axe = Instantiate(axe, bossHandTrans.position, this.transform.rotation);
        Axe.GetComponent<Axe>().SetPlayer(player);
    }
    public override void Dead()
    {
        isDead = true;
        GetComponent<Rigidbody>().isKinematic = false;
        animator.SetTrigger("Dead");
        animator.SetBool("IsDead", true);
        GameManager.Instance.BossAlive = false;
    }
}
