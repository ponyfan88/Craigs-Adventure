/* Programmer: Xander Mooney
 * Purpose: Control specifics of when the skeleton should do its behaviours
 * Inputs: the conditions of the AI
 * Outputs: what the AI should do
 */
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileSpawner), typeof(Animator))]
public class SkeletonStateManager : MonoBehaviour
{
    ProjectileSpawner projectileSpawner;
    Animator animator;
    int shotsCount;
    float timeTillNextShot = 0f;
    void Start()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Attack();
    }

    /*
    * purpose: make the skeleton throw bones at player
    * inputs: trigger to attack
    * outputs: bone bullets if conditions are met
    */
    void Attack()
    {
        // detect if the skeleton is able to shoot
        if (Time.time > timeTillNextShot)
        {
            // increment times the skeleton has shot
            ++shotsCount;
            // if the skeleton has shot less than specified amount asked
            if (shotsCount <= 2)
            {
                timeTillNextShot = Time.time + 3f;
                // Spawn bullet
                projectileSpawner.spawnerController(0);
            }
            else 
            { 
                // Make the skeleton move again
                animator.SetBool("reachedLocation", false);
                shotsCount = 0;
            }
        }
    }
}

