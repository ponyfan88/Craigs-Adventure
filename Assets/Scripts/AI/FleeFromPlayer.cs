using System;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromPlayer : StateMachineBehaviour
{
    GameObject player;
    NavMeshAgent ai;
    float distance;
    public float desiredDistance = 8;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player");
        ai = animator.gameObject.GetComponent<NavMeshAgent>();

        animator.SetBool("reachedLocation", false);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) player = GameObject.Find("player");
        distance = JMath.Distance(animator.gameObject.transform.position, player.transform.position);

        if (distance < desiredDistance)
        {
            Vector2 dir = animator.gameObject.transform.position - player.transform.position;

            dir = new Vector2(8,8) * JMath.Sign(dir) - dir;

            ai.destination = (Vector2)animator.gameObject.transform.position + dir;
        } 
        else
        {
            animator.SetBool("reachedLocation", true);
        }
    }
}
