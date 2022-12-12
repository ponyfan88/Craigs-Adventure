/* Programmer: Xander Mooney
 * Purpose: Control specifics of when the goblin should do its behaviours
 * Inputs: the conditions of the AI
 * Outputs: what the AI should do
 */

using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileSpawner),typeof(Animator),typeof(AIManager))]
public class goblinStateManager : MonoBehaviour
{
    #region Variables
    
    ProjectileSpawner projectileSpawner;
    Animator animator;
    byte timesSpit;
    float spitCooldown = 0f;
    
    #endregion

    #region Default Methods
    
    private void Awake()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
    }

    #endregion

    #region Custom Methods
    
    /*
     * purpose: Decide if goblin is able to attack
     * inputs: whether goblin can spit
     * outputs: spit if can spit
     */
    public void TrySpit()
    {
        if (spitCooldown < Time.time)
        {
            Spit();
        }
        else
        {
            animator.SetBool("reachedLocation", false);
        }
    }
    
    /*
     * purpose: make the goblin spit at player
     * inputs: trySpit()
     * outputs: projectilespawner.cs
     */
    public void Spit()
    {
        // increment consecutive times spit
        ++timesSpit;

        // spawn projectile
        projectileSpawner.spawnerController(0);
        
        // if we've spit 3 times consecutively, add a cooldown and force the man to walk
        if (timesSpit == 3)
        {
            animator.SetBool("reachedLocation", false);
            spitCooldown = Time.time + 2f;
            timesSpit = 0;
        }
    }

    #endregion
}
