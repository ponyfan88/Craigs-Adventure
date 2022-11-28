/* Programmer: Xander Mooney
 * Purpose: Apply damage to anything that collides with the corresponding object, at all times.
 * Inputs: 
 * Outputs:
 */
using UnityEngine;

public class DamageColliders : MonoBehaviour
{
    [Min(0)] public int DamageAmount = 1; // amount of damage it deals to non-player objects
    public bool DamagePlayer = true, DamageEnemies = true;
    Collider2D[] allOverlappingColliders;

    public void Start()
    {
        if (TryGetComponent(out BoxCollider2D boxCollider))
        {
            Vector2 center = transform.position;
            center += new Vector2(boxCollider.offset.x, boxCollider.offset.y);

            allOverlappingColliders = Physics2D.OverlapBoxAll(center, boxCollider.size, 0);
        }
        else if (TryGetComponent(out CircleCollider2D circleCollider))
        {
            Vector2 center = transform.position;
            center += new Vector2(circleCollider.offset.x, circleCollider.offset.y);

            allOverlappingColliders = Physics2D.OverlapCircleAll(center, circleCollider.radius);
        }

        foreach (Collider2D collider in allOverlappingColliders)
        {
            ApplyDamage(collider);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyDamage(collision);
    }

    private void ApplyDamage(Collider2D collision)
    {
        Debug.Log(collision.name + " Attempted Take Damage!");
        if (collision.TryGetComponent(out healthManager Enemyhealth))
        {
            Debug.Log(collision.name + " Took damage!");
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
}
