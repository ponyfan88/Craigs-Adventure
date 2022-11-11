
using UnityEngine;
using UnityEngine.AI;

public class getCloseToPlayer : StateMachineBehaviour
{
    GameObject player;
    NavMeshAgent ai;
    public Vector2 distance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player");
        ai = animator.gameObject.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Debug.Log(player.name.ToString());
        distance = animator.gameObject.transform.position - player.transform.position;

        if (distance.x < 4 && distance.y < 4)
        {
            ai.destination = ai.transform.position;
            animator.SetBool("reachedLocation", true);
        }
        else
            ai.destination = player.transform.position;
    }

}
