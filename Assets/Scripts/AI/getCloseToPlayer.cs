/* Programmer: Xander Mooney
 * Purpose: Get the AI as close to the player as specified
 * Inputs: ai and player position
 * Outputs: when to stop moving 
*/

using UnityEngine;
using UnityEngine.AI;

public class getCloseToPlayer : StateMachineBehaviour
{
    #region Variables

    GameObject player;
    NavMeshAgent ai;
    AIManager aiManager;
    public float desiredDistance = 4; 
    #endregion

    #region Default Methods

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player");
        ai = animator.gameObject.GetComponent<NavMeshAgent>();
        aiManager = animator.gameObject.GetComponent<AIManager>();

        aiManager.needDistance = true;
        animator.SetBool("reachedLocation", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reguarding any errors here, its likely you placed an enemy inside the starting room or a room without a navmesh

        // if within range, set the navMesh to not move and start attacking
        if (aiManager.distance <= desiredDistance)
        {
            // set our destination to literally ourself
            ai.destination = ai.transform.position;
            // we've reached our location, of course.
            animator.SetBool("reachedLocation", true);
        }
        else if (aiManager.canMove) // check if we can move
        {
            // if so, move to the player
            ai.destination = player.transform.position;
        }
        else
        {
            // otherwise, freeze in place
            ai.destination = ai.transform.position;
            // we've reached our location, of course.
            animator.SetBool("reachedLocation", true);
        }
    }

    #endregion

}
