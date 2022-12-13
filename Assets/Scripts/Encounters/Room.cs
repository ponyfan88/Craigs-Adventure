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
    [SerializeField] AnimationManager[] Decorations;
    SpawnEnemy[] enemySpawns;
    GenericNPC[] npcs;
    GlobalRoomManager roomManager;

    public int uniqueID = 0;

    bool enemiesSpawned = false, eventCancellatiion = false;

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
        doorManager = GetComponentsInChildren<DoorManager>();
        enemySpawns = GetComponentsInChildren<SpawnEnemy>();
        npcs = GetComponentsInChildren<GenericNPC>();
        roomManager = GetComponentInParent<GlobalRoomManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        eventCancellatiion = false;
        if (collision == playerHitbox && roomManager.inRoom == false) // the player has entered the room
        {
            foreach (DoorManager door in doorManager)
            {
                if (collision.IsTouching(door.GetComponent<Collider2D>()))
                eventCancellatiion = true;
            }
            if (!eventCancellatiion) 
            {
                roomManager.EnterRoom(); // tell global room manager that we are in a room

                if (TutorialManager.TutorialActive) //checks if tutorial is active
                {
                    roomManager.EnterEncounter(); // Tell global room manager that we are starting an encounter

                    // for every enemy that is a child of room child, spawn them in
                    foreach (SpawnEnemy spawn in enemySpawns)
                    {
                        // a number between 0 and 9
                        int a = (int)UnityEngine.Random.Range(0, FAILSPAWN_CHANCE);

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
                else if(!map.discovered.Contains(transform.parent.gameObject)) // if we havent entered this room before // --CAUSES A CRASH IN TUTORIAL-- no, it only causes a crash if the tutorial mode is disabled without changing the scene, which we plan on changing anyway
                {
                    roomHider.SetActive(true); // hides rooms (works on appropriate resolutions)

                    map.discovered.Add(transform.parent.gameObject); // add it to our list of entered rooms

                    roomManager.EnterEncounter(); // Tell global room manager that we are starting an encounter

                    // for every enemy that is a child of room child, spawn them in
                    foreach (SpawnEnemy spawn in enemySpawns)
                    {
                        // a number between 0 and 9
                        int a = (int)UnityEngine.Random.Range(0, FAILSPAWN_CHANCE);

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
                else
                {
                    roomHider.SetActive(true); // hides rooms (works on appropriate resolutions)
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
            roomManager.ExitRoom(); // tell the global room manager we left the room

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

            roomManager.ExitEncounter();

            //when the tutorial is active after enemies are defeated
            if (TutorialManager.TutorialActive)
            {
                //end tutorial
                TutorialManager.TutorialActive = false;
            }

            enemiesSpawned = false;
        }
    }

    #endregion
}