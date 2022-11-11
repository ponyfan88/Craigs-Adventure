/*
 * Programmers: Xander Mooney
 * Purpose: Make the AI properly face the direction its moving
 * Inputs: AI's NavmeshAgent
 * Outputs: if the sprite faces left or right
 */

using UnityEngine;
using UnityEngine.AI;

public class flipSprite : StateMachineBehaviour
{
    #region Variables

    NavMeshAgent ai;
    SpriteRenderer sprite;
    GameObject enemy;
    bool facingRight = true;

    #endregion

    #region Default Methods
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject;
        sprite = enemy.GetComponent<SpriteRenderer>();
        ai = enemy.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // if facing left and is moving right
        if (!facingRight && ai.desiredVelocity.x > 0)
        {
            Flip();
        }
       // if facing right and is moving left
        if (facingRight && ai.desiredVelocity.x < 0)
        {
            Flip();
        }
    }

    #endregion

    #region Custom Methods

    private void Flip()
    {
        // This will flip the sprite depending on which way its currently flipping.
        sprite.flipX = !sprite.flipX;
        facingRight = !facingRight;
    }

    #endregion
}
