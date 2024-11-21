using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject[] weakPoints;
    public int EnabledWeakPoints = 0;
    protected override void Update()
    {
        roarTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        FaceToPlayer();
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
    public void ShowWeakPoints()
    {
        int count = Mathf.Min(3 + level / 10, 7);
        List<int> numbers = new List<int>();
        for (int i = 0; i < weakPoints.Length; i++)
        {
            numbers.Add(i);
        }

        List<int> randomNumbers = new List<int>();

        while (randomNumbers.Count < count)
        {
            int index = Random.Range(0, numbers.Count);
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
        EnabledWeakPoints = 0;
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetActive(false);
        }
    }
    public override void GetHit(float damage)
    {
        if (isDead)
        {
            return;
        }
        currentHP -= damage;
        if (EnabledWeakPoints == 0)
        {
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");
            currentHP -= 3*damage;
        }
        if (currentHP <= 0)
        {
            Dead();
        }
    }
    public override void Dead()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        animator.SetBool("IsDead", true);
        GameManager.Instance.BossAlive = false;
    }
}
