/* Programmer: Xander Mooney
 * Purpose: Get the AI as close to the player as specified
 * Inputs: ai and player position
 * Outputs: when to stop moving 
*/
using System;
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

        // if within range, set the navMesh to not move and start attacking
        if (aiManager.distance <= desiredDistance)
        {
            ai.destination = ai.transform.position;
            animator.SetBool("reachedLocation", true);
            
        }
        else // check if we can move, and if so move to the player, otherwise freeze in place
            ai.destination = (aiManager.canMove == true ? player.transform.position : animator.gameObject.transform.position);
    }

    #endregion

}
