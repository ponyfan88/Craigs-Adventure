using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner))]
public class DragonStateManager : MonoBehaviour
{
    #region Variables

    Transform player;
    ProjectileSpawner projectileSpawner;
    public bool followPlayerX = true;
    const float speed = 2f;

    #endregion

    #region Default Methods

    void Awake()
    {
        player = GameObject.Find("player").transform;
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }

    void Update()
    {
        if (followPlayerX) // if the Ai currently asks for the dragon to follow the players X position
        {
            if (player.position.y > 7) // cancel movement if the player is beside the dragon; prevents clipping out of map or getting stuck
            {
                return;
            }
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
