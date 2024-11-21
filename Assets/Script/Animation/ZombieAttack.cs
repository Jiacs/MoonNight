using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : StateMachineBehaviour
{
    float timer;
    float attackDelay;
    bool hasHitted;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
        timer = 0;
        hasHitted = false;
        attackDelay = animator.GetComponent<Enemy>().attackDelay;
        if (animator.GetComponent<Boss>())
        {
            animator.GetComponent<Boss>().ShowWeakPoints();
            Time.timeScale -= 0.6f;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer >= attackDelay && !hasHitted)
        {
            animator.GetComponent<Enemy>().HitPlayer();
            hasHitted = true;
            if (animator.GetComponent<Boss>())
            {
                Time.timeScale += 0.6f;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Boss>()?.RemoveAllWeakPoints();
        if (animator.GetComponent<Boss>() && !hasHitted)
        {
            Time.timeScale += 0.6f;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
