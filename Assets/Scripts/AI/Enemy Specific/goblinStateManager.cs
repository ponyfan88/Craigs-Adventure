using UnityEngine;
public class goblinStateManager : MonoBehaviour
{
    #region Variables
    
    ProjectileSpawner projectileSpawner;
    GameObject player;
    Animator animator;
    Vector2 distance; // will be used later
    LayerMask raycastMask; // will be used later
    byte timesSpit;
    float spitCooldown = 0f;
    
    #endregion

    #region Default Methods
    
    private void Awake()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("player");

        // set the raycast's layermask to only include "IgnoreCollision"
        raycastMask = LayerMask.NameToLayer("IgnoreCollision");
        // using a bitwise operator to invert the layermask, meaning that the layer "IgnoreCollision" should be completely ignored.
        // raycastMask = ~raycastMask;
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
         distance = player.transform.position - transform.position;
        /* Physics2D.IgnoreLayerCollision(raycastMask, raycastMask);

        RaycastHit2D ray = Physics2D.Raycast(transform.position, distance);
        Debug.DrawRay(transform.position, distance, Color.red, 1f);

        if (ray.collider != null)
        {
            // Logic to avoid walls
            Debug.Log("Blocked shot by: " + ray.collider.name);
        }
        else
        {
            Spit();

            if (ray.collider.name != null)
                Debug.Log(ray.collider.name);
        } */


        // The code above WILL BE USED; its just raycasting is causing me issues with ignoring specific layers, for now i'm just gon make goblin spit

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
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void Spit()
    {
        // increment consecutive times spit
        ++timesSpit;

        // spawn projectile
        projectileSpawner.SpawnBullets();
        
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
