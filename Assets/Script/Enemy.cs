using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHP;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackCD;
    private float attackTimer;
    [SerializeField] private float roarCD;
    private float roarTimer;
    public float attackDelay;
    private float currentHP;
    private Animator animator;
    private bool isDead = false;
    private static GameObject player;
    private void Awake()
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
    void Update()
    {
        roarTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        FaceToPlayer();
        if(Vector3.Distance(this.gameObject.transform.position, player.transform.position) > attackDistance)
        {
            this.transform.forward = player.transform.position - this.transform.position;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            Attack();
        }
    }
    private void Attack()
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
    private void Roar()
    {
        if (roarTimer < roarCD)
        {
            return;
        }
        roarTimer = 0;
        animator.SetTrigger("Roar");
    }
    private void FaceToPlayer()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        dir.y = 0;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.3f);
    }
    public void GetHit(float damage)
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
    public void Dead()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        animator.SetBool("IsDead", true);
    }
}
