using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHP;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float attackCD;
    protected float attackTimer;
    [SerializeField] protected float roarCD;
    [SerializeField] protected float moveSpeed;
    protected float roarTimer;
    public float attackDelay;
    protected float currentHP;
    protected Animator animator;
    protected bool isDead = false;
    protected static GameObject player;
    protected int level;
    protected void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        roarTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        FaceToPlayer();
        if(Vector3.Distance(this.gameObject.transform.position, player.transform.position) > attackDistance)
        {
            this.transform.forward = player.transform.position - this.transform.position;
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * moveSpeed;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            Attack();
        }
    }
    protected virtual void Attack()
    {
        if(attackTimer<attackCD)
        {
            Roar();
        }
        else
        {
            attackTimer = 0;
            animator.ResetTrigger("Roar");
            animator.SetTrigger("Attack");
        }
    }
    public void HitPlayer()
    {
        player.GetComponent<Player>().GetHit();
    }
    protected void Roar()
    {
        if (roarTimer < roarCD)
        {
            return;
        }
        roarTimer = 0;
        animator.SetTrigger("Roar");
    }
    protected void FaceToPlayer()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        dir.y = 0;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.3f);
    }
    public virtual void GetHit(float damage)
    {
        if(isDead)
        {
            return;
        }
        currentHP -= damage;
        animator.ResetTrigger("GetHit");
        animator.SetTrigger("GetHit");
        if(currentHP <= 0)
        {
            Dead();
        }
    }
    public virtual void Dead()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        animator.SetBool("IsDead", true);
        GameManager.zombieCount--;
    }
    public virtual void SetLevel(int level)
    {
        this.level = level;
        currentHP *= 1 + 0.1f * level;
    }
}
