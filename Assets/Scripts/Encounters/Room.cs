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
    [SerializeField] private AnimationManager[] Decorations;
    [SerializeField] private SpawnEnemy[] enemySpawns;
    bool enemiesSpawned = false;

    #endregion

    #region Default Methods

    private void Awake()
    {
        playerHitbox = GameObject.Find("player").GetComponent<CapsuleCollider2D>(); // get the player as a CapsuleCollider2D
        // this helps us detect if it was specifically the player, rather than their attack or an item.

        map = FindObjectOfType<Map>(); //find map

        doorManager = transform.GetComponentsInChildren<DoorManager>();
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
            foreach (AnimationManager decoration in Decorations)// if there are any animated decoration this allows them to be enabled and disable if the player not in the room
            {
                decoration.beginAnimation();
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
            foreach (AnimationManager decoration in Decorations)
            {
                decoration.endAnimation();
            }
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