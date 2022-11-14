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
        knockbackDir = new Vector2(Math.Sign(transform.position.x - position.position.x), Math.Sign(transform.position.y - position.position.y));
        knockbackTimer = Time.time + knockbackTime;
    }
    #endregion
}
