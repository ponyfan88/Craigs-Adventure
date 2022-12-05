/* 
 * Programmer: Xander Mooney
 * Purpose: Manage which doors are open and closed
 * Inputs: Whether a room has been entered
 * Outputs: Closes and opens doors
 */

using UnityEngine;

public class DoorManager : MonoBehaviour
{
    #region Variables

    Animator animator;
    BoxCollider2D Collider;

    #endregion

    #region Default Methods
    
    public void Awake()
    {
        animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();
    }

    #endregion

    #region Custom Methods
    
    /*
     * purpose: Open the door for the player to go through them
     * inputs: the door to open
     * outputs: opens door
     */
    public void OpenDoors()
    {
        animator.SetBool("isOpen", true);
        Collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
    }
    
    /*
     * purpose: Clost the door to stop the player
     * inputs: the door to close
     * outputs: closes door
     */
    public void CloseDoors()
    {
        animator.SetBool("isOpen", false);
        Collider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    #endregion
}
