using UnityEngine;

public class SelectRandState : StateMachineBehaviour
{
    public int States = 2;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        System.Random rand = new System.Random();
        animator.SetInteger("State", rand.Next(1, States + 1));
    }
}
