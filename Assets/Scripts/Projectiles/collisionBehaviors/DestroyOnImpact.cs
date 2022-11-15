/* Programmer: Xander Mooney
 * Purpose: make bullets and throwable objects destroy on impact with walls or other colliders
 * Inputs: if a collision happens
 * Outputs: Destroys the object
*/
using UnityEngine;
public class DestroyOnImpact : MonoBehaviour
{
    [Min(0)]public int DamageAmount = 1; // amount of damage it deals to non-player objects
    public bool playerOwned = false, destroyOnHitWall = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled)
        {
            bool hasHealth = collision.gameObject.TryGetComponent(out healthManager colHealth);
            // if the collider is the player and it is specifically collider with the capsule collider, which is detected to avoid colliding with the attack hitbox
            if (!playerOwned && collision.gameObject.name == "player" && collision.gameObject.GetComponent<CapsuleCollider2D>().IsTouching(gameObject.GetComponent<Collider2D>()))
            {
                bool tookDamage = colHealth.TakeDamage(1, false); // deal only 1 damage
                if (tookDamage)
                {
                    // If the object colliding is a projectile (determined by if it has projectile script), we knockback based on the objects rotation, otherwise we use the position
                    if (TryGetComponent(out Projectile objProjectile))
                    {
                        collision.gameObject.GetComponent<controller>().ApplyKnockback(objProjectile.rotation);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<controller>().ApplyKnockback(transform.position);
                    }
                    gameObject.SetActive(false); // only destroy the object if damage was applied
                }
            }
            // the collider is owned by the player, and collision has health so we need to damage it
            else if (playerOwned && hasHealth && collision.gameObject.name != "player" && collision.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision"))
            {
                if (collision.TryGetComponent(out AIManager aiManager))
                {
                    aiManager.ApplyKnockback(transform.position);
                }
                bool tookDamage = colHealth.TakeDamage(DamageAmount, false);
                gameObject.SetActive(false);
            }
            // Collider does not have health, so its probably a wall and you should destroy it
            else if (collision.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision") && collision.gameObject.name != "player")
            {
                // if we want the object to destory on hitting the wall, we disable the object, otherwise we do not (so we set it to true)
                gameObject.SetActive(destroyOnHitWall == true ? false : true);
            }

        }
    }
}
