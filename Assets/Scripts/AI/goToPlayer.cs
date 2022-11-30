/* 
 * Programmers: Xander Mooney
 * Purpose: Move the AI towards the player
 * Inputs: Navmesh agent and player
 * Outputs: Movement towards the player
 */

using UnityEngine;
using UnityEngine.AI;

public class goToPlayer : StateMachineBehaviour
{
    #region Variables

    NavMeshAgent ai;
    AIManager aiManager;
    GameObject player;

    #endregion

    #region Default Methods
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // This script requires these components to be found on the AI, otherwise it won't function properly
        ai = animator.gameObject.GetComponent<NavMeshAgent>();
        aiManager = animator.gameObject.GetComponent<AIManager>();
        player = GameObject.Find("player"); // Finds the player
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if the ai is able to move, set location to the players position, otherwise set it to its own position.
        ai.destination = (aiManager.canMove == true ? new Vector2(player.transform.position.x, player.transform.position.y) : animator.gameObject.transform.position);
    }

    #endregion
}
