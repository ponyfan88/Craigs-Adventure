
using UnityEngine;
using UnityEngine.AI;

public class getCloseToPlayer : StateMachineBehaviour
{
    #region Variables

    GameObject player;
    NavMeshAgent ai;
    AIManager aiManager;
    public float distance;

    #endregion

    #region Custom Methods

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player");
        ai = animator.gameObject.GetComponent<NavMeshAgent>();
        aiManager = animator.gameObject.GetComponent<AIManager>();
    }

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the distance from the player
        distance = JMath.Distance(animator.gameObject.transform.position, player.transform.position);

        // if within range, set the navMesh to not move and start attacking
        if (distance < 4)
        {
            ai.destination = ai.transform.position;
            animator.SetBool("reachedLocation", true);
            
        }
        else // check if we can move, and if so move to the player, otherwise freeze in place
            ai.destination = (aiManager.canMove == true ? player.transform.position : animator.gameObject.transform.position);
    }

    #endregion

}
