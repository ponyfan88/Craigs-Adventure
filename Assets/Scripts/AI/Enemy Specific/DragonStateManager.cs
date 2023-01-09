/*
 * Programmers: Xander Mooney
 * Purpose: Control specifics of when the Dragon should move and do its behaviours
 * Inputs: the conditions of the dragon
 * Outputs: what the dragon should do
 */
using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner))]
public class DragonStateManager : MonoBehaviour
{
    #region Variables

    Transform player;
    ProjectileSpawner projectileSpawner;
    public bool followPlayerX = true, attacking = false;
    const float speed = 2f, attackCooldown = 1f;
    private float nextAttackTime;

    #endregion

    #region Default Methods

    void Awake()
    {
        player = GameObject.Find("player").transform;
        projectileSpawner = GetComponent<ProjectileSpawner>();

        nextAttackTime = Time.time + attackCooldown * 2;
    }

    void Update()
    {
        if (!attacking && Time.time > nextAttackTime)
        {
            followPlayerX = false;
        }

        // if the Ai currently asks for the dragon to follow the players X position, and the player isn't too high up
        if (followPlayerX && player.position.y < 7)
        {
            // calculate distance from player
            float dist = JMath.Distance(transform.position.x, player.position.x);

            // if the dragon can move without moving past the player then move towards the player
            if (dist > speed * Time.deltaTime)
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
            // check in negative direction
            else if (dist < -(speed * Time.deltaTime))
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
        }
    }
    #endregion

    #region Custom Methods

    /* Purpose: Allow the animator to call a function to spawn the dragons fireball attack
     * Input: When to attack
     * Output: Attack
     */
    void Fireball()
    {
        projectileSpawner.spawnerController(0);
    }

    #endregion
}
