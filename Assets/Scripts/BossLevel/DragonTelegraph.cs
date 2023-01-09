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
    void SummonMeteor()
    {
        // spawns projectile
        projSpawner.spawnerController(0);

        Destroy(gameObject);
    }

    #endregion
}
