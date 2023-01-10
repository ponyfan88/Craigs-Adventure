/* Programmer: Xander Mooney
 * Purpose: make bullets and throwable objects destroy on impact with walls or other colliders
 * Inputs: if a collision happens
 * Outputs: Destroys the object
*/
using UnityEngine;
using UnityEditor;
public class DestroyOnImpact : MonoBehaviour
{
    #region Variables

    [Min(0)]public int DamageAmount = 1; // amount of damage it deals to non-player objects
    public bool playerOwned = false, destroyOnHitWall = true;

    // enum to decide what should happen if an object were to explode.
    public enum DestroyType { destroy, summonProjectile };
    public DestroyType destroyType = DestroyType.destroy;
    public int projectileIndex= 0;//the projectile to spawn when destroyed

    #endregion

    #region Classes
#if (UNITY_EDITOR)
    /* Custom class written within the script to handle the inspector
     * Allows us to customize the inspector with code
     */
    [CustomEditor(typeof(DestroyOnImpact))]
    public class DestroyOnImpactManager : Editor
    {
        SerializedProperty damageAmount, playerOwned, destroyOnHitWall, destroyType, projectileIndex;

        private void OnEnable()
        {
            damageAmount = serializedObject.FindProperty("DamageAmount");
            playerOwned = serializedObject.FindProperty("playerOwned");
            destroyOnHitWall = serializedObject.FindProperty("destroyOnHitWall");
            destroyType = serializedObject.FindProperty("destroyType");
            projectileIndex = serializedObject.FindProperty("projectileIndex");
        }


        public override void OnInspectorGUI()
        {
            var DestroyOnImpact = (DestroyOnImpact)target;

            // call an update to the inspector objects
            serializedObject.Update();
            // Tell main properties to update
            EditorGUILayout.PropertyField(damageAmount);
            EditorGUILayout.PropertyField(playerOwned);
            EditorGUILayout.PropertyField(destroyOnHitWall);
            EditorGUILayout.PropertyField(destroyType);

            // Make projectileIndex only appear if destroyType needs it
            if (DestroyOnImpact.destroyType == DestroyType.summonProjectile)
            {
                EditorGUILayout.PropertyField(projectileIndex);
            }

            // Apply any changes the user makes to the properties  THIS SHOULD ALWAYS BE AT THE END
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion

    #region Default Methods

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnTriggerStay2D(collision.collider);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled) // run nothing if disabled
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
                    DestroyObject(); // only destroy the object if damage was applied
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
                DestroyObject();
            }
            // Collider does not have health, so its probably a wall and you should destroy it
            else if (collision.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision") && collision.gameObject.name != "player")
            {
                // if we want the object to destory on hitting the wall, we disable the object, otherwise we do not (so we set it to true)
                if (destroyOnHitWall)
                    DestroyObject();
            }
        }
    }

    #endregion

    #region Custom Methods

    /*
     * Purpose: Run through enums to determine how to destroy the object
     * Inputs: when destroyed
     * Outputs: custom destruction implementations
     */
    public void DestroyObject()
    {
        switch (destroyType)
        {
            case DestroyType.summonProjectile:
                if (TryGetComponent(out ProjectileSpawner projectileSpawner))
                {
                    projectileSpawner.spawnerController(projectileIndex);
                    destroyType = DestroyType.destroy;
                }
                break;
            case DestroyType.destroy:
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    #endregion
}
