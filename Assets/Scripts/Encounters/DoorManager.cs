/* Programmer: Xander Mooney
 * Purpose: Manage which doors are open and closed
 * Inputs: Whether a room has been entered
 * Outputs: Closes and opens doors
 */
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    Animator animator;
    BoxCollider2D Collider;
    public void Awake()
    {
        animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();
    }
    public void OpenDoors()
    {
        animator.SetBool("isOpen", true);
        Collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
    }
    
    public void CloseDoors()
    {
        animator.SetBool("isOpen", false);
        Collider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
