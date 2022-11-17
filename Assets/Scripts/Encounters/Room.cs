/* 
 * Programmers: Jack Kennedy and Christopher Kowalewski 
 * Purpose: script that runs for each room; does various things
 * Inputs: collisions to the room
 * Outputs: various variables to other script files such as map.cs
 */

using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    #region Variables

    public GameObject roomHider; // the room hider

    CapsuleCollider2D playerHitbox; // the player's capsule collider

    public int roomType; // our rooms "type" (where it opens, where there are walls, etc.)
    Map map; // our minimap

    DoorManager[] doorManager;

    private SpawnEnemy[] enemySpawns;
    [SerializeField]bool enemiesSpawned = false;

    #endregion

    #region Default Methods

    private void Awake()
    {
        playerHitbox = GameObject.Find("player").GetComponent<CapsuleCollider2D>(); // get the player as a CapsuleCollider2D
        // this helps us detect if it was specifically the player, rather than their attack or an item.

        map = FindObjectOfType<Map>(); //find map

        // this will make 2 seperate array's with any object with either the script "DoorManager" or "SpawnEnemy" as to access them later.
        doorManager = transform.GetComponentsInChildren<DoorManager>();
        enemySpawns = transform.GetComponentsInChildren<SpawnEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerHitbox) // the player has entered the room
        {

            roomHider.SetActive(true); // hides rooms (works on appropriate resolutions)

            if (!map.discovered.Contains(transform.parent.gameObject)) // if we havent entered this room before
            {
                map.discovered.Add(transform.parent.gameObject); // add it to our list of entered rooms

                foreach (SpawnEnemy spawn in enemySpawns)
                {
                    spawn.Spawn(transform.parent);
                }
            }
            foreach (DoorManager door in doorManager)
            {
                door.CloseDoors();
            }

            enemiesSpawned = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == playerHitbox) // the player has left the room
        {
            roomHider.SetActive(false); // hide our room hider
        }
    }

    private void Update()
    {
        if (enemiesSpawned)
        {
            foreach (SpawnEnemy spawn in enemySpawns)
            {
                if (spawn.enemy != null)
                {
                    return;
                }
            }

            foreach (DoorManager door in doorManager)
            {
                door.OpenDoors();
            }
        }
    }

    #endregion
}