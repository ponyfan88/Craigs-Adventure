/* Programmer: Xander Mooney
 * Purpose: Apply damage to anything that collides with the corresponding object, at all times.
 * Inputs: 
 * Outputs:
 */
using UnityEngine;

public class DamageColliders : MonoBehaviour
{
    #region Variables

    [Min(0)] public int DamageAmount = 1; // amount of damage it deals to non-player objects
    public bool DamagePlayer = true, DamageEnemies = true;
    Collider2D[] allOverlappingColliders;

    #endregion

    #region Default Methods

    public void Start()
    {
        // A recurring issue is that on the initialization of a component, any Collider already within the bounds of an object does not trigger "OnTriggerEnter2D", meaning objects won't take damage.
        // To fix this, on start; which is ran when the object is initialized, we recreate the colliders size and apply damage that way.

        // Test if the component has a boxCollider
        if (TryGetComponent(out BoxCollider2D boxCollider))
        {
            // find the center of the gameObject and then offset it by the colliders offset
            Vector2 center = transform.position;
            center += new Vector2(boxCollider.offset.x, boxCollider.offset.y);

            // Add all overlapping colliders to an array
            allOverlappingColliders = Physics2D.OverlapBoxAll(center, boxCollider.size, 0);
        }
        // Object does not have a box collider, so test Circle collider. We do not test any other colliders as we've only used circle and box colliders thus far
        else if (TryGetComponent(out CircleCollider2D circleCollider))
        {
            Vector2 center = transform.position;
            center += new Vector2(circleCollider.offset.x, circleCollider.offset.y);

            allOverlappingColliders = Physics2D.OverlapCircleAll(center, circleCollider.radius);
        }

        // loop through every collider in array, and try to damage it
        foreach (Collider2D collider in allOverlappingColliders)
        {
            ApplyDamage(collider);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Try to apply damage to any collider that enters its radius
        ApplyDamage(collision);
    }

    #endregion

    #region Custom Methods

    /* Purpose: Apply damage to every collider that has health
     * Inputs: colliders overlapping area
     * Outputs: Damages colliders
     */
    private void ApplyDamage(Collider2D collision)
    {
        // test if object has health
        if (collision.TryGetComponent(out healthManager Enemyhealth))
        {
            // if object is not player 
            if (DamageEnemies && collision.name != "player")
            {
                Enemyhealth.TakeDamage(DamageAmount);
            }
            if (DamagePlayer && collision.name == "player")
            {
                Enemyhealth.TakeDamage(1);
            }
        }
    }
    /* Purpose: Allow us to destroy the object when needed
     * Inputs: When to be destroyed
     * Outputs: Destroys object
     */
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    #endregion
}
