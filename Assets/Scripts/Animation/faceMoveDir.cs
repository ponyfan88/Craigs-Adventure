/* 
 * Programmers: Xander Mooney
 * Purpose: Make the AI's animation look like its facing the direction in which it is moving
 * Inputs: NavmeshAgent and the objects move direction
 * Outputs: info into the animator of which way it should make the animation look
 */

using System;
using UnityEngine;
using UnityEngine.AI;

public class faceMoveDir : StateMachineBehaviour
{
    #region Variables

    GameObject animatedObject;
    NavMeshAgent ai;
    Vector2 moveDir;
    
    #endregion

    #region Default methods
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatedObject = animator.gameObject;
        ai = animatedObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // This finds how the AI is moving, and then makes it a positive integer for comparing in the if statement.
        moveDir = new Vector2(Mathf.Abs(ai.desiredVelocity.x), Mathf.Abs(ai.desiredVelocity.y));

        if (moveDir.x > moveDir.y) // If the object is moving more along the X axis than the Y axis
        {
            // By setting both values to false, we have the animator assume that its moving sideways
            animator.SetBool("moveUp", false);
            animator.SetBool("moveDown",false);
        } else if (ai.desiredVelocity.y > 0) // if you are moving upwards vertically
        {
            animator.SetBool("moveUp",true);
            animator.SetBool("moveDown", false);
        } else // if you are moving downwards vertically
        {
            animator.SetBool("moveUp", false);
            animator.SetBool("moveDown", true);
        }
    }

    #endregion
}
