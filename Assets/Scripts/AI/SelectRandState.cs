/*
 * Programmer: Xander Mooney
 * Purpose: Allow the animator to select a random state, so that it can do a random attack or pattern
 * Inputs: # of states
 * Outputs: random state #
 */
using UnityEngine;

public class SelectRandState : StateMachineBehaviour
{
    [TextArea]
    public string Note = "Requires a float attribute named 'State' within the animator to work";
    public int States = 2;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        System.Random rand = new System.Random();
        animator.SetInteger("State", rand.Next(1, States + 1));
    }
}
