using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileSpawner),typeof(Animator),typeof(AIManager))]
public class goblinStateManager : MonoBehaviour
{
    #region Variables
    
    ProjectileSpawner projectileSpawner;
    GameObject player;
    Animator animator;
    byte timesSpit;
    float spitCooldown = 0f;
    
    #endregion

    #region Default Methods
    
    private void Awake()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("player");
    }

    #endregion

    #region Custom Methods
    
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
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
