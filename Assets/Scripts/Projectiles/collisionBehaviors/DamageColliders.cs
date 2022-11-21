using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliders : MonoBehaviour
{
    [Min(0)]public int DamageAmount = 1; // amount of damage it deals to non-player objects
    public bool DamagePlayer = true, DamageEnemies = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out healthManager Enemyhealth))
        {
            if (DamageEnemies && collision.name != "player")
            {
                Enemyhealth.TakeDamage(DamageAmount);
            }
            else if (DamagePlayer && collision.name == "player")
            {
                Enemyhealth.TakeDamage(1);
            }
            
        }
    }
}
