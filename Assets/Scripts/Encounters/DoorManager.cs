/* Programmer: Xander Mooney
 * Purpose: Manage which doors are open and closed
 * Inputs: Whether a room has been entered
 * Outputs: Closes and opens doors
 */
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoors()
    {
        animator.SetBool("isOpen", true);
    }
    
    public void CloseDoors()
    {
        animator.SetBool("isOpen", false);
    }
}
