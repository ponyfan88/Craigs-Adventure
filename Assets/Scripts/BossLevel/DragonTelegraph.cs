/* Programmer: Xander Mooney
 * Purpose: Control the time to summon the meteor for the telegraphed attack
 * Input: Time to summon meteor
 * Output: Meteor
 */
using UnityEngine;

public class DragonTelegraph : MonoBehaviour
{
    #region Variables

    ProjectileSpawner projSpawner;

    #endregion

    #region Default Methods
    private void Awake()
    {
        // get projectile spawner component
        projSpawner = GetComponent<ProjectileSpawner>();
    }

    /*
     * Purpose: summon meteor when animator calls
     * Input: Timed event
     * Output: summom meteor
     */
    void SummonMeteor()
    {
        // spawns projectile
        projSpawner.spawnerController(0);


        Invoke("KillObject", 1f);
    }

    /*
     * Purpose: kill the telegraph after an amount of time to make sure the projectile spawned properly
     * Input: Invoke
     * Output: kill object
     */
    void KillObject()
    {
        Destroy(gameObject);
    }

    #endregion
}
