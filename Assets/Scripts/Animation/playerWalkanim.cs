/*
 * Programmers: Xander Mooney
 * Purpose: Make the player output animations based on how its moving
 * Inputs: controller.cs and attack.cs variables
 * Outputs: Information into the animatior to animate the character properly
 */

using UnityEngine;

public class playerWalkanim : StateMachineBehaviour
{
    #region  Variables

    controller controller;
    Attack attack;
    itemManager itemManager;
    public bool facingRight = true, wasHoldingItem = false;
    
    #endregion

    #region Default Methods
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<controller>(); // get the controller script
        attack = animator.gameObject.GetComponent<Attack>(); // get the attacking script
        itemManager = animator.gameObject.GetComponent<itemManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!Pause.paused) // prevent animation changing while paused
        {
            if (controller.xMov != 0 || controller.yMov != 0) // if x/y movement is not 0, you are moving
            {           
                animator.SetBool("isMoving", true);

                if (controller.moveDirection.x != 0) // if you are moving X, we want to prioritize it over Y
                {
                    animator.SetBool("facingUp", false);
                    animator.SetBool("facingDown", false);
                }
                else if (controller.moveDirection.y > 0) // if you are moving up
                {
                    animator.SetBool("facingUp", true);
                    animator.SetBool("facingDown", false);
                }
                else // if you are moving down
                {
                    animator.SetBool("facingUp", false);
                    animator.SetBool("facingDown", true);
                }
            }
            else animator.SetBool("isMoving", false); // not moving
            {
                if (controller.dashMoveCooldown > Time.time) // if the cooldown is larger than the current game time, then we want the dash animation to be playing
                {
                    animator.SetBool("isDashing", true);
                    animator.SetBool("isMoving", false);
                }
                else
                {
                    animator.SetBool("isDashing", false); // not dashing
                }

                // if facing right, but velocity is left or if facing left, but velocity is right
                if ((facingRight && controller.xMov < 0) || (!facingRight && controller.xMov > 0))
                {
                    FlipDir();
                } else if (!wasHoldingItem && itemManager.holdingItem)
                {
                    itemManager.FlipObject(facingRight);
                }
            }
        }
    }

    #endregion

    #region Custom Methods

    void FlipDir() // flips the characters sprite along the X axis
    {
        // as long as we arent attacking
        if (!attack.isAttacking)
        {
            controller.gameObject.GetComponent<SpriteRenderer>().flipX = !controller.gameObject.GetComponent<SpriteRenderer>().flipX; // set the object to flip opposite way
            facingRight = !facingRight; // flip which way it says we are facing

            if (itemManager.holdingItem)
                itemManager.FlipObject(facingRight);
        }
    }

    #endregion
}
