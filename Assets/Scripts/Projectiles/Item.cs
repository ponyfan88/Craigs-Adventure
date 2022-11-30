/* 
 * Programmers: Xander Mooney
 * Purpose: define an object as interactble with the player, and then feed information about it to the players Pick-up Manager
 * Inputs: Information about the projectile
 * Outputs: Whether the object meets the criteria to be picked up by the player
 */

using UnityEngine;

public class Item : MonoBehaviour
{
    #region Variables

    Transform playerPos;
    itemManager itemMan;
    public DestroyOnImpact collisionScript;
    float Distance;
    public Vector2 holdingOffset;
    GameObject player;
    public float throwVelocity = 0.1f;
    public bool canBeGrabbed = true, hasCollision = false;

    

    #endregion

    #region Default Methods

    void Awake()
    {
        // reads if the object is meant to be destroyed on impact
        hasCollision = TryGetComponent<DestroyOnImpact>(out collisionScript);
        
        if (hasCollision) // if the object will destroy on collision, we need to disable it to let the player pick it up in the first place
        {
            collisionScript.playerOwned = true;
            collisionScript.enabled = false;
        }

        playerPos = GameObject.Find("player").transform;
        player = GameObject.Find("player");
        itemMan = playerPos.gameObject.GetComponent<itemManager>();
    }
    void Update()
    {
        if (canBeGrabbed) // canBeGrabbed can be manually changed or changed by other scripts
        {
            Distance = JMath.Distance(playerPos.position, transform.position); // finds the distance from the player
            if (Distance <= 1) // if the distance from the player is within the pickup radius (1)
            {
                // feed the objects information and position into the players item manager to determine if it is the closest object to the player
                itemMan.SelectedPickup(transform.gameObject, Distance);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the collision is not the player, and not set to ignore collisions, tell the item Manager to stop its movement
        if (collision.gameObject != player && collision.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision"))
        {
            itemMan.CollideItem(transform.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // if the collision is not the player, and not set to ignore collisions, tell the item Manager to stop its movement
        if (collision.gameObject != player && collision.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision"))
        {
            itemMan.CollideItem(transform.gameObject);
        }
    }

    #endregion
}
