using System;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromPlayer : StateMachineBehaviour
{
    GameObject player;
    NavMeshAgent ai;
    AIManager aiManager;
    public float desiredDistance = 8;
    float timeSinceFlee;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player");
        ai = animator.gameObject.GetComponent<NavMeshAgent>();
        aiManager = animator.GetComponent<AIManager>();
        aiManager.needDistance = true;
        animator.SetBool("reachedLocation", false);

        timeSinceFlee = Time.time;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (aiManager.distance < desiredDistance && Time.time - timeSinceFlee <= 3)
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
