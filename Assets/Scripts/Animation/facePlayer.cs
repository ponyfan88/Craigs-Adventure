/* 
 * Programmers: Xander Mooney
 * Purpose: Feed information into the animator to make it always face towards the player
 * Inputs: the player and AIs positions
 * Outputs: the direction the AI needs to face to make it look at the player via animation
 */

using System;
using UnityEngine;
using UnityEngine.AI;

public class facePlayer : StateMachineBehaviour
{
    #region Variables

    GameObject animatedObject;
    Transform player;
    Vector2 posOffset, posOffsetAbs;
    
    #endregion

    #region Default Methods
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatedObject = animator.gameObject;
        player = GameObject.Find("player").transform; //transform of the player

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Calculates the offset from the players pos to the AI's pos, and again for the postitive values of it.
        posOffset = player.position - animatedObject.transform.position;
        posOffsetAbs = new Vector2(Math.Abs(posOffset.x),Math.Abs(posOffset.y));
        
        if (posOffsetAbs.x > posOffsetAbs.y) // if the player is placed more to the sides of the AI than above or below it
        {
            //By setting both values to false, the animator can assume it is moving sideways
            animator.SetBool("moveUp", false);
            animator.SetBool("moveDown", false);
        }
        else if (posOffset.y > 0) // if the player is above the AI
        {
            animator.SetBool("moveUp", true);
            animator.SetBool("moveDown", false);
        }
        else // if the player is below the AI
        {
            animator.SetBool("moveUp", false);
            animator.SetBool("moveDown", true);
        }
    }

    #endregion
}
