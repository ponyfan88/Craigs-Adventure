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
        player = GameObject.Find("player");
        
        // we get the navmesh controller (agent), and then make it so it doesn't rotate or change its axis.
        // this is important, as otherwise it would rotate in a way where its "invisible" to the player
        ai = GetComponent<NavMeshAgent>();
        ai.updateRotation = false;
        ai.updateUpAxis = false;
    }

    private void Update()
    {
        // if knockbackTimer is > the current time, it is within frames where it is being knocked back
        if (knockbackTimer > Time.time)
        {
            // We reset the AI's path, this prevents some bugs where the AI completely ignores taking knockback
            // The NavMeshAgent is a really weird with how its velocity works, meaning there are some specific bugs where enemies refuse to take knockback
            // I've done tons of research into fixing this issue however it is not completely preventable due to Unity's implemenation.
            // This can be seen with the Goblin enemy not taking knockback vertically. - Xander
            ai.ResetPath();
            ai.velocity = new Vector2(knockbackDir.x * (6 - knockbackResistence), knockbackDir.y * (6 - knockbackResistence));
        }

        // if the AI needs distance, it can specify it in its behaviour scripts. This allows us to reuse the same calculation many times.
        if (needDistance)
        {
            distance = JMath.Distance((Vector2)transform.position, (Vector2)player.transform.position);
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
