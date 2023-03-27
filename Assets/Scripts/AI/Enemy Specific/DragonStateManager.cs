/*
 * Programmers: Xander Mooney
 * Purpose: Control specifics of when the Dragon should move and do its behaviours
 * Inputs: the conditions of the dragon
 * Outputs: what the dragon should do
 */

using UnityEngine;

[RequireComponent(typeof(ProjectileSpawner),typeof(Animator),typeof(healthManager))]
public class DragonStateManager : MonoBehaviour
{
    #region Variables

    Transform player;
    ProjectileSpawner projectileSpawner;
    Animator animator;
    healthManager dragonHealth;
    public GameObject DragonTelegraph; // manually added in the inspector
    public bool followPlayerX = true, attacking = false;
    const float speed = 2f, attackCooldown = 4f;
    private float nextAttackTime;
    byte attackCount = 0;

    #endregion

    #region Default Methods

    void Awake()
    {
        player = GameObject.Find("player").transform;
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
        dragonHealth = GetComponent<healthManager>();

        nextAttackTime = Time.time + attackCooldown;
    }

    void Update()
    {
        if (!attacking && Time.time > nextAttackTime)
        {
            followPlayerX = false;
            animator.SetBool("Attacking", true);
        }

        if (followPlayerX)
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

    /*
     * Purpose: Allow the animator to call a function to spawn the dragons fireball attack
     * Input: When to attack
     * Output: Attack
     */
    void Fireball()
    {
        if (attackCount <= 3)
        {
            projectileSpawner.spawnerController(0);
            ++attackCount;
        }
        else
        {
            ResetAttack();
        }
    }
    /*
     * Purpose: Allow the animator to call a function to determine if it should telegraph an attack
     * Input: When to attack
     * Output: Attack
     */
    void Telegraph()
    {
        if (animator.GetBool("Attacking"))
        {
            if (attackCount <= 5)
            {
                Instantiate(DragonTelegraph, player.position - new Vector3(0, 0.5f), new Quaternion(0, 0, 0, 0));

                if (dragonHealth.health <= dragonHealth.maxHealth / 2)
                {
                    System.Random rand = new System.Random();
                    Instantiate(DragonTelegraph, player.position - new Vector3(rand.Next(-5,6), rand.Next(-5,6)), new Quaternion(0,0,0,0));
                    Instantiate(DragonTelegraph, player.position - new Vector3(rand.Next(-5, 6), rand.Next(-5, 6)), new Quaternion(0, 0, 0, 0));
                }
                ++attackCount;
            }
            else
            {
                ResetAttack();
            }
        }
    }

    /*
     * Purpose: reset Dragons attack cooldown and make him idel
     * Input: when to reset attacks
     * Output: reset attacks
     */
    void ResetAttack()
    {
        animator.SetBool("Attacking", false);
        followPlayerX = true;
        attackCount = 0;
        nextAttackTime = Time.time + attackCooldown;
    }
    #endregion
}
