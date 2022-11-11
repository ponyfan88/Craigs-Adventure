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
        Debug.Log("Door Open triggered");
        animator.SetBool("isOpen", true);
    }
    
    public void CloseDoors()
    {
        Debug.Log("Door Close triggered");
        animator.SetBool("isOpen", false);
    }
}
