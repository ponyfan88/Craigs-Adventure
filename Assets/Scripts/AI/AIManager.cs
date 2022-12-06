/* 
 * Programmers: Xander Mooney
 * Purpose: store data that all enemy AI scripts can access on one enemy.
 * Inputs: NavMesh data, Knockback information
 * Outputs: whether the AI can move, knockback velocity
 */

using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[DisallowMultipleComponent]
public class AIManager : MonoBehaviour
{
    #region Variables
    
    // variable(s) to be accessed across all ai scripts
    public bool canMove = true;
    [NonSerialized] public bool needDistance = false;
    [NonSerialized] public float distance;
    [Range(0,6)] public float knockbackResistence = 0f;
    const float KNOCKBACKTIME = 0.0525f;
    float knockbackTimer = 0f;
    Vector2 knockbackDir;
    NavMeshAgent ai;
    GameObject player;

    #endregion

    #region Default Methods

    private void Awake()
    {
        // we get the navmesh controller (agent), and then make it so it doesn't rotate or change its axis.
        // this is important, as otherwise it would rotate in a way where its "invisible" to the player
        ai = GetComponent<NavMeshAgent>();
        player = GameObject.Find("player");

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

        if (needDistance)
        {
            distance = JMath.Distance(transform.position, player.transform.position);
        }
    }

    #endregion

    #region Custom Methods
    
    /*
     * purpose: Apply knockback to the ai when called
     * inputs: Direction attacked from (max 1, min -1)
     * outputs: knockback direction
     */
    public void ApplyKnockback(Vector2 AttackDir)
    {
        knockbackDir = new Vector2(Math.Sign(AttackDir.x), Math.Sign(AttackDir.y));
        knockbackTimer = Time.time + KNOCKBACKTIME;
    }
    
    /*
     * purpose: Apply knockback to the ai when called
     * inputs: transform of object dealing damage
     * outputs: knockback direction
     */
    public void ApplyKnockback(Transform position)
    {
        knockbackDir = new Vector2(Math.Sign(position.position.x - transform.position.x), Math.Sign(position.position.y - transform.position.y));
        knockbackTimer = Time.time + KNOCKBACKTIME;

    }
    
    /*
     * purpose: Apply knockback to the ai when called
     * inputs: rotation of object dealing damage
     * outputs: knockback direction
     */
    public void ApplyKnockback(float bulletRotation)
    {
        // turns rotation of the object into a radian       
        float radian = Mathf.Deg2Rad * bulletRotation;
        // uses cosine and sine to turn the radian of the bullet into a direction to push the player
        knockbackDir = new Vector2(Math.Sign(Mathf.Cos(radian)), Math.Sign(Mathf.Sin(radian)));
        knockbackTimer = Time.time + KNOCKBACKTIME;
    }
    
    #endregion
}
