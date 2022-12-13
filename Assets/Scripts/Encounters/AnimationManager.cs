/* 
 * Programmers: Anmol Acharya
 * Purpose: script that runs starts and stops animating 
 * Inputs: 
 * Outputs: animation starting or stoping
 */

using UnityEngine;

//IMPORTANT MAKE SURE ALL Animators that have this script use the name "on" the name of the boolean
public class AnimationManager : MonoBehaviour
{

    #region Variables

    Animator animator;
    private void Start()
    {
       animator = GetComponent<Animator>();

    }
    #endregion

    #region Custiom Methods
    /*
     * purpose: begin animation
     * inputs: entering a room
     * outputs: the object starting animation
     */
    public void beginAnimation()
    {
        animator.SetBool("on", true);
    }
    
    /*
     * purpose: end animation when player leaves a room
     * inputs: player exiting the room
     * outputs: the object stopping animation
     */
    public void endAnimation() 
    {
        animator.SetBool("on",false);
        
    }
    #endregion
}
