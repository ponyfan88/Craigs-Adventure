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

    Pause Pause; // import pausing

    public int spawnCount;
    public int roomAmt = 20;
    [SerializeField] private rTemplate template;
    [SerializeField] private NavMeshSurface2d navMesh;

    private List<rGen> spawnList = new List<rGen>();
    private rGen[] spawns;
    private byte rand;
    private bool built = false;

    SavesManager savesManager; // our saves manager
    FloorManager floorManager; // our floor manager

    #endregion

    #region Default Methods

    public void Awake()
    {
        savesManager = FindObjectOfType<SavesManager>(); //assign the saves manager

        Pause = GameObject.Find("UI").GetComponent<Pause>(); // find our pause script
        Pause.StartLoading(); // we've started loading our game

        floorManager = FindObjectOfType<FloorManager>();

        if (savesManager.loadingSave && !floorManager.loadSaveOverride) // if we are loading a save
        {
            LogToFile.Log("we are using an existing seed: " + savesManager.currentSave.seed); // lets say so in a log!
            Random.InitState(savesManager.currentSave.seed); // set our random seed
        }
        else
        {
            // this is our random seed. this generates practically random numbers.
            savesManager.currentSave.seed = (int)System.DateTime.Now.Ticks;
            // this logs our random seed to our log file
            LogToFile.Log("we've generated a new seed: " + savesManager.currentSave.seed.ToString());
            // make sure we actually use this number
            Random.InitState((int)savesManager.currentSave.seed);

            floorManager.loadSaveOverride = false;
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
        Instantiate<GameObject>(template.Boss[spawnList[rand].openingDirection - 1], spawnList[rand].transform.position, template.Boss[spawnList[rand].openingDirection - 1].transform.rotation, gameObject.transform);
    }
    private void genWalls() //adds walls to places walls should be
    {
        //gen walls for rGen conflict queue or didn't spawn because of limit
        for (int i = 0; i < spawns.Length; ++i) //for each spawnpoint (in the entire game)
        {
            if (spawns[i].needWall || !spawns[i].spawned) //if the spawn needs a wall
            {
                Instantiate<GameObject>(template.Walls[spawns[i].openingDirection - 1], spawns[i].gameObject.transform.position, template.Walls[spawns[i].openingDirection - 1].transform.rotation, gameObject.transform); //copy the prefab for the wall into the grid
            }
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