/* 
 * Programmers: Christopher Kowalewski, Jack Kennedy
 * Purpose: Finalize anything related to room/dungeon generation
 * Inputs: rGen calls run, as well as takes information from every spawnpoint (objects with rGen script in them)
 * Outputs: Instantiates Final room and walls, Bakes navMesh, and both shows/hides the loading screen.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class globalGen : MonoBehaviour
{
    #region Variables

    ThingManager thingManager;

    // spawnrooms
    [SerializeField] private GameObject spawnRoom1;
    [SerializeField] private GameObject spawnRoom2;

    Pause Pause; // import pausing

    Map map; //find the map script

    public int spawnCount;
    public int roomAmt = 20;
    [SerializeField] private rTemplate template;
    [SerializeField] private NavMeshSurface2d navMesh;

    private List<rGen> spawnList = new List<rGen>();
    private rGen[] spawns;
    private byte rand;
    private bool built = false;

    SavesManager savesManager; // our saves manager

    SpawnEnemy[] enemySpawns; //storage for all enemy spawnpoints in every room
    GameObject[][] floorEnemies; //a template

    private float timer;

    #endregion

    #region Default Methods

    public void Awake()
    {
        thingManager = FindObjectOfType<ThingManager>();

        //fill out a jagged array of enemies for each floor. Order: [floor number][enemy prefab]
        floorEnemies = new GameObject[2][];
        floorEnemies[0] = new GameObject[] { thingManager.slimePrefab, thingManager.goblinPrefab };
        floorEnemies[1] = new GameObject[] { thingManager.slimePrefab, thingManager.goblinPrefab, thingManager.skeletonPrefab, thingManager.wizardPrefab };

        timer = Time.time; //stores time at game start (in case infinite loading occurs, we need to stop it at some point)

        map = FindObjectOfType<Map>();

        if (FloorManager.floor == 1)
        {
            GameObject spawnRoom = Instantiate(spawnRoom2, transform, true); //spawn in the spawn room
            map.discovered.Add(spawnRoom); //add it to the map
        }
        else
        {
            GameObject spawnRoom = Instantiate(spawnRoom1, transform, true); //spawn in the spawn room
            map.discovered.Add(spawnRoom); //add it to the map
        }
        
        savesManager = FindObjectOfType<SavesManager>(); //assign the saves manager

        Pause = GameObject.Find("UI").GetComponent<Pause>(); // find our pause script
        Pause.StartLoading(); // we've started loading our game

        if (savesManager.loadingSave && !FloorManager.loadSaveOverride) // if we are loading a save
        {
            LogToFile.Log("we are using an existing seed: " + savesManager.currentSave.seed); // lets say so in a log!
            Random.InitState(savesManager.currentSave.seed); // set our random seed
        }
        else
        {
            // this is our random seed. this generates practically random numbers.
            savesManager.currentSave.seed = Mathf.Sqrt(System.DateTime.Now.Ticks.Bottom()).LargerTillInt();
            // this logs our random seed to our log file
            LogToFile.Log("we've generated a new seed: " + savesManager.currentSave.seed.ToString());
            // make sure we actually use this number
            Random.InitState((int)savesManager.currentSave.seed);

            FloorManager.loadSaveOverride = false;
        }
    }

    private void Update()
    {
        if (timer <= Time.time - 20f && !built)
        {
            Debug.Log("This is taking a while, retrying generation.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); //reload scene
        }
    }

    #endregion

    #region Custom Methods

    public void run()
    {
        if (spawnCount >= roomAmt && !built) //once the rooms have generated to a certain number of them
        {
            built = true; //dont run twice check

            Invoke("genEnd", 0.05f); //final room (staircase)
            Invoke("genWalls", 0.15f); //extra walls
            Invoke("setEnemies", 0.2f); //set the enemy types spawning on this floor
            Invoke("genNav", 0.25f); //ai pathing (bakes navmesh)
            Invoke("endLoading", 1f); //unpauses the game
        }
    }
    private void genEnd() //generates an end room (staircases for now)
    {
        spawns = gameObject.GetComponentsInChildren<rGen>(); //neatly finds every spawnpoint in the game

        for (int i = 0; i < spawns.Length; i++) //loops through every spawn
        {
            if (!spawns[i].spawned) //if it wasnt spawned
            {
                spawnList.Add(spawns[i]);
            }
        }
        rand = (byte)Random.Range(0, spawnList.Count); //decide a random room spawn point
        spawnList[rand].spawned = true; //set the specific spawnpoint to be "spawned" so it doesn't request a wall
        GameObject endRoom = Instantiate(template.Boss[spawnList[rand].openingDirection - 1], spawnList[rand].transform.position, template.Boss[spawnList[rand].openingDirection - 1].transform.rotation, gameObject.transform);
        endRoom.tag = "End Room";
    }

    private void genWalls() //adds walls to places walls should be
    {
        //gen walls for rGen conflict queue or didn't spawn because of limit
        for (int i = 0; i < spawns.Length; ++i) //for each spawnpoint (in the entire game)
        {
            if (spawns[i].needWall || !spawns[i].spawned) //if the spawn needs a wall
            {
                Instantiate<GameObject>(template.Walls[spawns[i].openingDirection - 1], spawns[i].gameObject.transform.position, template.Walls[spawns[i].openingDirection - 1].transform.rotation, spawns[i].transform); //copy the prefab for the wall into the grid
            }
        }
    }

    private void setEnemies() //sets the enemies to spawn on each floor within rooms with enemy spawnpoints
    {
        enemySpawns = gameObject.GetComponentsInChildren<SpawnEnemy>(); //get all enemy spawnpoints

        for (int i = 0; i < enemySpawns.Length; ++i)
        {
            enemySpawns[i].enemiesTemplate = floorEnemies[FloorManager.floor - 1];
        }
    }

    private void genNav() //bakes a navmesh for ai pathfinding
    {
        navMesh.BuildNavMesh();
    }
    private void endLoading()
    {
        Pause.EndLoading();
    }

    #endregion
}