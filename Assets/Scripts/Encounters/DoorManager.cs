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
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void OpenDoors()
    {
        animator.SetBool("isOpen", true);
        Collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
    }
    
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void CloseDoors()
    {
        animator.SetBool("isOpen", false);
        Collider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    #endregion
}
