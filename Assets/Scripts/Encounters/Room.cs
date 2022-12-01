/* 
 * Programmers: Jack Kennedy and Christopher Kowalewski 
 * Purpose: script that runs for each room; does various things
 * Inputs: collisions to the room
 * Outputs: various variables to other script files such as map.cs
 */

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
    private SpawnEnemy[] enemySpawns;
    private GenericNPC[] npcs;
    bool enemiesSpawned = false;

    //% chance for an enemy not to spawn --- 1/x chance to fail, so 10 is 10%, 20 is 5%, etc.
    private const int FAILSPAWN_CHANCE = 8;

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
        npcs = transform.GetComponentsInChildren<GenericNPC>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerHitbox) // the player has entered the room
        {

            roomHider.SetActive(true); // hides rooms (works on appropriate resolutions)

            if (!map.discovered.Contains(transform.parent.gameObject)) // if we havent entered this room before
            {
                map.discovered.Add(transform.parent.gameObject); // add it to our list of entered rooms

                // for every enemy that is a child of room child, spawn them in
                foreach (SpawnEnemy spawn in enemySpawns)
                {
                    // a number between 0 and 9
                    int a = (int)Random.Range(0, FAILSPAWN_CHANCE);

                    // if the number is 0 dont spawn the enemy (10% chance not to spawn)
                    if (a != 0)
                    {
                        // spawn 90% of the time
                        spawn.Spawn(transform.parent);
                    }
                    else
                    {
                        // log that we didnt spawn an enemy
                        LogToFile.Log("enemy did not spawn due to random chance");
                    }
                }
            }

            // if there are any animated decoration this allows them to be enabled and disable if the player not in the room
            foreach (AnimationManager decoration in Decorations)
            {
                decoration.beginAnimation();
            }

            // close every door in this room
            foreach (DoorManager door in doorManager)
            {
                door.CloseDoors();
            }

            // if theres an npc in this room, make them start talking
            foreach (GenericNPC npc in npcs)
            {
                npc.EnterRoom();
            }

            enemiesSpawned = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if its the player that has left the room
        if (collision == playerHitbox)
        {
            roomHider.SetActive(false); // hide our room hider
            
            // for every decoration, stop their animation, since the player wont be seeing them anyways
            foreach (AnimationManager decoration in Decorations)
            {
                decoration.endAnimation();
            }

            // for every npc run the exitroom method, stopping all dialogue
            foreach (GenericNPC npc in npcs)
            {
                npc.ExitRoom();
            }
        }
    }

    private void Update()
    {
        // if our enemies have already spawned
        if (enemiesSpawned)
        {
            // for every enemy
            foreach (SpawnEnemy spawn in enemySpawns)
            {
                // check if the enemy is dead
                if (spawn.enemy != null)
                {
                    // if it ISNT dead, stop this method
                    return;
                }
            }

            // otherwise, every enemy in this room is dead, so we can open the doors
            
            // for every door, open
            foreach (DoorManager door in doorManager)
            {
                door.OpenDoors();
            }
        }
    }

    #endregion
}