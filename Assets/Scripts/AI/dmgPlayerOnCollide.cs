/* 
 * Programmers: Xander Mooney
 * Purpose: detect if the player is colliding with an object which deals damage, and knock them back appropriately.
 * Inputs: if collision happens, all the player information, and the enemies information
 * Outputs: knockback for the player
 */

using UnityEngine;

public class dmgPlayerOnCollide : MonoBehaviour
{
    #region Variables

    healthManager playerHealth;
    controller playerController;
    AIManager aiManager;
    
    #endregion
    
    #region Default Methods

    private void Awake()
    {
        aiManager = GetComponent<AIManager>();
        playerHealth = GameObject.Find("player").GetComponent<healthManager>();
        playerController = GameObject.Find("player").GetComponent<controller>();
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.name == "player") // Find if the collision is from the player
        {
            //apply damage and knockback
            bool tookDamage = playerHealth.TakeDamage(1, false); 
            if (tookDamage) // tookDamage is true if the .TakeDamage function was able to apply damage
            {
                // apply knockback and freeze the AI
                playerController.ApplyKnockback(this.transform.position);
                aiManager.canMove = false;
                Invoke("ToggleAI", 0.3f);
            }
        }
    }

    #endregion

    #region Custom Methods

    private void ToggleAI() // toggle ai's ability to move
    {
        aiManager.canMove = !aiManager.canMove;
    }

    #endregion
}
