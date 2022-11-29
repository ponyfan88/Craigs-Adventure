/* 
 * Programmers: Christopher Kowalewski 
 * Purpose: Spawns a room from the parent gameObject if possible
 * Inputs: rTemplate stores the prefabs for this script. Uses a globalGen function and variables. 
 * Outputs: sFrom and openingDirection are int values indicating direction used in multiple scripts (incluidng this and other rGens) (continues next line)
 * spawned and needWall is used in globalGen to determine where walls are needed. Instantiates rooms, which if the spawn cap isnt reached, spawn more rooms.
 */

using UnityEngine;

public class rGen : MonoBehaviour
{
    #region Variables

    public byte sFrom; //spawned FROM (set by previous room) see opening Direction for more
    public byte openingDirection; //1-4, 1 = this spawn faces up/north, 2 = down/south, 3 = right/east, 4 = left/west
    public bool spawned = false; //can spawn a room if false, true if it spawned a room OR conflicts in another way
    public bool needWall = false; //Requests a wall from globalGen
    
    [SerializeField] private Rigidbody2D colCheck; //collision detection
    //[SerializeField] private byte quad = 0; //corner of a 2 by 2

    private GameObject grid; //for the overarching grid that rooms are put onto 
    private rTemplate templates; //for the prefab references
    private globalGen global; //global generation manager script
    private byte rand; //random number
    private GameObject clone; //identifying variable for the new room spawned (if any)

    SavesManager savesManager; // variable for the saves manager

    /*
    //2 by 2 room testSpawns
    private GameObject testSpawn1;
    private GameObject testSpawn2;
    private GameObject testSpawn3;
    //chosen 2 by 2 actual spawning point
    private GameObject spawn2;

    private bool failed2by2 = false;
    */

    #endregion

    #region Default Methods

    private void Start()
    {
        grid = FindObjectOfType<Grid>().gameObject; //assign the grid
        templates = grid.GetComponent<rTemplate>(); //assign the templates
        global = grid.GetComponent<globalGen>(); //assign the global generation manager
        savesManager = FindObjectOfType<SavesManager>(); //assign the saves manager
        //note: this is all so we can reference these in the script (very useful)

        Random.InitState(savesManager.currentSave.seed);

        LogToFile.Log("our current seed: " + savesManager.currentSave.seed);

        if (global.spawnCount < global.roomAmt) //max room count
        {
            //for deciding to spawn a 2by2 (at random)
            /*rand = (byte)Random.Range(1, 5);*/

            Invoke("Spawn", 0.05f); //below max room count
        }
        else
        {
            global.run(); //at or above max room count (runs globalGen)
        }
    }

    private void OnTriggerEnter2D(Collider2D col) //if an object exists in the spawn point position
    {
        if (col.CompareTag("Spawnpoint")) //collides with another spawn
        {
            if (!spawned) //this spawnpoint has not spawned a room
            {
                switch (sFrom) //based on spawned from direction
                {
                    case 1:
                        if (openingDirection != 2) //if this spawn is not over the room that spawned it
                        {
                            spawned = true; //probably doesnt have this set, so we set it
                            needWall = true; //need wall here
                        }
                        break;
                    case 2:
                        if (openingDirection != 1) //if this spawn is not over the room that spawned it
                        {
                            spawned = true; //same as above
                            needWall = true; //need wall
                        }
                        break;
                    case 3:
                        if (openingDirection != 4) //if this spawn is not over the room that spawned it
                        {
                            spawned = true; //same as above
                            needWall = true; //need wall
                        }
                        break;
                    case 4:
                        if (openingDirection != 3) //if this spawn is not over the room that spawned it
                        {
                            spawned = true; //same as above
                            needWall = true; //need wall
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else if (col.CompareTag("killSpawn")) //Kill Spawn Hitbox
        {
            if (!spawned) //if THIS spawn point is not "spawned"
            {
                spawned = true; //immediately set spawned to true;
                switch (sFrom) //spawned from cases
                {
                    case 1: //from below going up
                    case 2: //from above going down
                        if (openingDirection == 3 || openingDirection == 4) //left or right
                        {
                            needWall = true; //request wall
                        }
                        break;
                    case 3: //from left going right
                    case 4: //from right going left
                        if (openingDirection == 1 || openingDirection == 2) //left or right
                        {
                            needWall = true; //request wall
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion

    #region Custom Methods
    private void Spawn() //function for spawning a room
    {
        if (!spawned) //as long as spawned is false for various reasons
        {
            /*
            if (rand == 4)
            {
                switch (openingDirection)
                {
                    case 1:
                        testSpawn1 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x, transform.position.y + 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn2 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y + 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn3 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y, 0), templates.testSpawns[0].transform.rotation, grid.transform);

                        spawn2 = testSpawn1;

                        Invoke("spawn2by2", 0.02f);
                        break;
                    case 2:
                        testSpawn1 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn2 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn3 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y, 0), templates.testSpawns[0].transform.rotation, grid.transform);

                        spawn2 = gameObject;

                        Invoke("spawn2by2", 0.02f);
                        break;
                    case 3:
                        testSpawn1 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn2 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn3 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x + 20, transform.position.y, 0), templates.testSpawns[0].transform.rotation, grid.transform);

                        spawn2 = gameObject;

                        Invoke("spawn2by2", 0.02f);
                        break;
                    default:
                        testSpawn1 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn2 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x - 20, transform.position.y - 20, 0), templates.testSpawns[0].transform.rotation, grid.transform);
                        testSpawn3 = Instantiate<GameObject>(templates.testSpawns[0], new Vector3(transform.position.x - 20, transform.position.y, 0), templates.testSpawns[0].transform.rotation, grid.transform);

                        spawn2 = testSpawn3;

                        Invoke("spawn2by2", 0.02f);
                        break;
                }
            }
            if (rand != 4 || failed2by2)
            { */
                switch (openingDirection) //an int value that determines what door matches the spawnpoint (to connect the rooms properly)
                {
                    case 1:
                        //Spawn a BOTTOM door room
                        rand = (byte)Random.Range(0, templates.bRooms.Count);
                        clone = Instantiate<GameObject>(templates.bRooms[rand], transform.position, templates.bRooms[rand].transform.rotation, grid.transform);
                        templates.bRooms.RemoveAt(rand);
                        break;
                    case 2:
                        //Spawn a TOP door room
                        rand = (byte)Random.Range(0, templates.tRooms.Count);
                        clone = Instantiate<GameObject>(templates.tRooms[rand], transform.position, templates.tRooms[rand].transform.rotation, grid.transform);
                        templates.tRooms.RemoveAt(rand);
                        break;
                    case 3:
                        //Spawn a LEFT door room
                        rand = (byte)Random.Range(0, templates.lRooms.Count);
                        clone = Instantiate<GameObject>(templates.lRooms[rand], transform.position, templates.lRooms[rand].transform.rotation, grid.transform);
                        templates.lRooms.RemoveAt(rand);
                        break;
                    case 4:
                        //Spawn a RIGHT door room
                        rand = (byte)Random.Range(0, templates.rRooms.Count);
                        clone = Instantiate<GameObject>(templates.rRooms[rand], transform.position, templates.rRooms[rand].transform.rotation, grid.transform);
                        templates.rRooms.RemoveAt(rand);
                        break;
                    default:
                        clone = gameObject; //never happens, but code complains if clone is never set
                        break;
                }
                rGen[] spawns = clone.GetComponentsInChildren<rGen>(); //every rGen in the spawnpoints that are children of the new room

                for (int i=0; i < spawns.Length; ++i) //loop through the spawns in new room
                {
                    spawns[i].sFrom = openingDirection; //set the "spawned From" direction
                }

                ++global.spawnCount; //adds to rooms spawned
                spawned = true; //marks as spawned
            /*}*/
        }
    }
    
    /*private void spawn2by2()
    {
        if ((testSpawn1.GetComponentInChildren<testSpawn>().conflict == false) && (testSpawn2.GetComponentInChildren<testSpawn>().conflict == false) && (testSpawn3.GetComponentInChildren<testSpawn>().conflict == false))
        {
            clone = Instantiate<GameObject>(templates.mutliRooms[0], spawn2.transform.position, templates.mutliRooms[0].transform.rotation, grid.transform);

            rGen[] spawns = clone.GetComponentsInChildren<rGen>(); //every rGen in the spawnpoints that are children of the new room

            for (int i = 0; i < spawns.Length; ++i) //loop through
            {
                switch (spawns[i].quad)
                {
                    case 1:
                        spawns[i].sFrom = openingDirection;
                        break;
                    case 2:
                        switch (openingDirection)
                        {
                            case 1:
                            case 2:
                                spawns[i].sFrom = 3;
                                break;
                            case 3:
                            case 4:
                                spawns[i].sFrom = openingDirection;
                                break;
                            default:
                                Debug.Log(openingDirection.ToString());
                                break;
                        }
                        break;
                    case 3:
                        spawns[i].sFrom = 1;
                        break;
                    case 4:
                        if (openingDirection == 1)
                        {
                            spawns[i].sFrom = 1;
                        }
                        else
                        {
                            spawns[i].sFrom = 3;
                        }
                        break;
                    default:
                        Debug.Log(spawns[i].quad.ToString());
                        break;
                }

                //spawns[i].sFrom = openingDirection; //set the "spawned From" direction
            }

            ++global.spawnCount; //adds to rooms spawned
            spawned = true; //marks as spawned
        }
        else
        {
            failed2by2 = true; //indicate that a 2by2 is not possible in current location so a 1x1 is spawned instead
            Spawn(); //retry spawning
        }
    }*/

    #endregion
}
