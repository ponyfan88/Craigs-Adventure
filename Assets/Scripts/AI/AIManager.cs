/* 
 * Programmers: Xander Mooney
 * Purpose: store data that all enemy AI scripts can access on one enemy.
 * Inputs: NavMesh data, Knockback information
 * Outputs: whether the AI can move, knockback velocity
 */

using System;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    #region Variables
    
    // variable(s) to be accessed across all ai scripts
    public bool canMove = true;
    [Range(0,6)] public float knockbackResistence = 0f;
    float knockbackTime = 0.0525f, knockbackTimer = 0f;
    Vector2 knockbackDir;
    NavMeshAgent ai;

    #endregion

    #region Default Methods

    private void Awake()
    {
        // we get the navmesh controller (agent), and then make it so it doesn't rotate or change its axis.
        // this is important, as otherwise it would rotate in a way where its "invisible" to the player
        ai = GetComponent<NavMeshAgent>();
        ai.updateRotation = false;
        ai.updateUpAxis = false;
    }

    private void Update()
    {
        if (knockbackTimer > Time.time)
        {
            ai.ResetPath();
            ai.velocity = new Vector2(knockbackDir.x * (6 - knockbackResistence), knockbackDir.y * (6 - knockbackResistence));
        }
    }

    #endregion

    #region Custom Methods
    public void ApplyKnockback(Vector2 AttackDir)
    {
        knockbackDir = new Vector2(Math.Sign(AttackDir.x), Math.Sign(AttackDir.y));
        knockbackTimer = Time.time + knockbackTime;
    }
    public void ApplyKnockback(Transform position)
    {
        knockbackDir = new Vector2(Math.Sign(position.position.x - transform.position.x), Math.Sign(position.position.y - transform.position.y));
        knockbackTimer = Time.time + knockbackTime;

    }
    public void ApplyKnockback(float bulletRotation)
    {
        // turns rotation of the object into a radian       
        float radian = Mathf.Deg2Rad * bulletRotation;
        // uses cosine and sine to turn the radian of the bullet into a direction to push the player
        knockbackDir = new Vector2(Math.Sign(Mathf.Cos(radian)), Math.Sign(Mathf.Sin(radian)));
        knockbackTimer = Time.time + knockbackTime;
    }
    #endregion
}
