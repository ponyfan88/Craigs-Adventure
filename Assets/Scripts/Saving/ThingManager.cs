/* 
 * Programmers: Jack Kennedy
 * Purpose: manages items and enemies
 * Inputs: the save we are loading / items & enemies in scene
 * Outputs: sometimes spawns in inputs, other times destroys them
 */

using System.Collections.Generic;
using UnityEngine;

public class ThingManager : MonoBehaviour
{
    #region Variables

    // privates we dont share (:<

    private SavesManager savesManager;

    private Room[] rooms;

    private List<GameObject> destroyGameObjects = new List<GameObject>();

    // our prefabs

    public GameObject bombPrefab;
    public GameObject bonePrefab;
    public GameObject bookPrefab;
    public GameObject book2Prefab;
    public GameObject boxPrefab;
    public GameObject heartJarPrefab;
    public GameObject lanternPrefab;
    public GameObject moyaiPrefab;
    public GameObject logPrefab;

    public GameObject goblinPrefab;
    public GameObject skeletonPrefab;
    public GameObject slimePrefab;
    public GameObject fireSlimePrefab;
    public GameObject wizardPrefab;

    #endregion

    #region Default Methods

    // Awake is called after the first frame update
    void Awake()
    {
        savesManager = FindObjectOfType<SavesManager>();

        // if we are loading a save
        if (savesManager.loadingSave)
        {
            // reconstruct after 1 second
            Invoke("ReconstructGenericObjects", 1f);

            // invoking prevents an old bug where we'd get less rooms than if we waited till room gen is finished
        }
    }

    #endregion

    #region Custom Methods

    /* 
     * Purpose: loops through in-scene items & enemies, can save or destroy them
     * Inputs: save them? destroy them? should we loop through in scene or those from our save? 
     * Outputs: sometimes saves objects to our currentSave, other times destroys them. sometimes both. 
     */
    public void NukeRoomChildren(bool save = false, bool destroy = true, bool loadFromSave = false)
    {
        if (!loadFromSave)
        {
            // EVERY OBJECT CALLED ROOM CHILD 
            rooms = FindObjectsOfType<Room>();
            // but also the room script component

            // create a list of the rooms themselves
            List<GameObject> roomGameObjects = new List<GameObject>();

            // create a list of room ids
            List<int> uniqueIDs = new List<int>();

            // fill that list with each rooms parent
            foreach (Room room in rooms)
            {
                // add our unique id to our list of unique ids
                uniqueIDs.Add(room.uniqueID);
                // we take the transform of our room child, find the transform, get the parent of the transform, and get the gameobject of the parent
                roomGameObjects.Add(room.transform.parent.gameObject);
            }

            // we will be storing items and enemies as generic objects
            List<GenericObject> genericObjects = new List<GenericObject>();
            // we dont test for saving since Visual Studio will yell at you for not putting this here

            // if we are saving clear our list of gameobjects to destroy
            if (save)
            {
                destroyGameObjects = new List<GameObject>();
            }

            // counter to loop through rooms
            int counter = 0;

            // for every room on grid
            foreach (GameObject room in roomGameObjects)
            {
                // for every child
                foreach (Transform child in room.transform) // things like items, spawns, and enemies
                {
                    // our child's tag
                    string tag = child.gameObject.tag;

                    // if its an enemy or an item, thats what we're looking to save
                    if (tag == "Item" || tag == "Enemy")
                    {
                        // if we are not just destroying objects
                        if (save)
                        {
                            // we make a new GenericObject
                            GenericObject genericObject = new GenericObject();

                            // the itemEnemyThing is the item/enemy we are looping throuhg
                            genericObject.thingPrefab = child.gameObject.GetComponent<Thing>().thingPrefab;

                            genericObject.uniqueID = uniqueIDs[counter];

                            genericObject.position = child.gameObject.transform.position;

                            genericObject.health = child.gameObject.transform.GetComponent<healthManager>().health;

                            // add it to our list of game objects
                            genericObjects.Add(genericObject);

                            destroyGameObjects.Add(child.gameObject);
                        }

                        if (destroy)
                        {
                            Destroy(child.gameObject); // destroy the item/enemy gameobject (they'll be replaced later)
                        }
                    }
                }

                ++counter;
            }

            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            Debug.Log(rootObjects.Length);

            foreach (GameObject rootObject in rootObjects)
            {
                // our child's tag
                string tag = rootObject.tag;

                // if its an enemy or an item, thats what we're looking to save
                if (tag == "Item" || tag == "Enemy")
                {
                    // if we are not just destroying objects
                    if (save)
                    {
                        // we make a new GenericObject
                        GenericObject genericObject = new GenericObject();

                        // the itemEnemyThing is the item/enemy we are looping throuhg
                        genericObject.thingPrefab = rootObject.GetComponent<Thing>().thingPrefab;

                        genericObject.uniqueID = -1;

                        genericObject.position = rootObject.transform.position;

                        genericObject.health = rootObject.transform.GetComponent<healthManager>().health;

                        // add it to our list of game objects
                        genericObjects.Add(genericObject);

                        destroyGameObjects.Add(rootObject);

                        Debug.Log(genericObject);
                    }

                    if (destroy)
                    {
                        Destroy(rootObject); // destroy the item/enemy gameobject (they'll be replaced later)
                    }
                }

                ++counter;
            }
            // if we are not just destroying objects
            if (save)
            {
                // store our generic objects in our save
                savesManager.currentSave.genericObjects = genericObjects;
            }
        }
        else
        {
            if (destroy)
            {
                foreach (GameObject destroyMe in destroyGameObjects)
                {
                    Destroy(destroyMe);
                }

                NukeRoomChildren(save, false);
            }
        }
    }

    /* 
     * Purpose: reconstructs various objects items and enemies
     * Inputs: the save we are loading / items & enemies in scene
     * Outputs: sometimes spawns in inputs, other times destroys them
     */
    public void ReconstructGenericObjects()
    {
        if (savesManager.currentSave.genericObjects == null) // this will only come up null if we arent loading a save properly (corruption?)
        {
            LogToFile.Log("prefab saves loaded, currentSave contained none");
            NukeRoomChildren(true, true);
        }
        else
        {
            LogToFile.Log("prefabs found, loading currentSave");
            NukeRoomChildren(false, true);
        }
        
        rooms = FindObjectsOfType<Room>();
        // EVERY OBJECT CALLED ROOM CHILD 
        // but also the room script component

        // create a list of the rooms themselves
        List<GameObject> roomGameObjects = new List<GameObject>();

        // fill that list with each rooms parent
        foreach (Room room in rooms)
        {   
            // we take the transform of our room child, find the transform, get the parent of the transform, and get the gameobject of the parent
            roomGameObjects.Add(room.transform.parent.gameObject);
        }


        // for every saved object in savesManager
        for (int i = 0; i < savesManager.currentSave.genericObjects.Count; ++i)
        {
            // our current generic object from our current save
            GenericObject genericObject = savesManager.currentSave.genericObjects[i];

            // predab we instantiate later
            GameObject prefab;
            
            // what type is our object?
            switch (genericObject.thingPrefab)
            {
                // ITEMS
                case (thingEnums.thingPrefab.bomb):
                    prefab = bombPrefab;
                    break;
                case (thingEnums.thingPrefab.bone):
                    prefab = bonePrefab;
                    break;
                case (thingEnums.thingPrefab.book):
                    prefab = bookPrefab;
                    break;
                case (thingEnums.thingPrefab.book2):
                    prefab = book2Prefab;
                    break;
                case (thingEnums.thingPrefab.box):
                    prefab = boxPrefab;
                    break;
                case (thingEnums.thingPrefab.heartJar):
                    prefab = heartJarPrefab;
                    break;
                case (thingEnums.thingPrefab.lantern):
                    prefab = lanternPrefab;
                    break;
                case (thingEnums.thingPrefab.moyai):
                    prefab = moyaiPrefab;
                    break;
                case (thingEnums.thingPrefab.log):
                    prefab = logPrefab;
                    break;
                // ENEMIES
                case (thingEnums.thingPrefab.goblin):
                    prefab = goblinPrefab;
                    break;
                case (thingEnums.thingPrefab.skeleton):
                    prefab = skeletonPrefab;
                    break;
                case (thingEnums.thingPrefab.slime):
                    prefab = slimePrefab;
                    break;
                case (thingEnums.thingPrefab.wizard):
                    prefab = wizardPrefab;
                    break;
                case (thingEnums.thingPrefab.redSlime):
                    prefab = fireSlimePrefab;
                    break;
                // DEFAULT
                default:
                    prefab = null; // spawn nothing if its an object we dont recognise
                    break;
            }

            // if our prefab isnt a null value (aka we are actually supposed to spawn something)
            if (prefab != null)
            {
                // instantilize with proper position
                prefab.transform.position = genericObject.position;

                // instantilize with proper health
                prefab.GetComponent<healthManager>().health = genericObject.health;

                int uniqueID = genericObject.uniqueID;

                if (uniqueID == -1)
                {
                    Debug.Log("found object with no parent: " + prefab.name);
                    // instantilize with no parent
                    GameObject thing = Instantiate(prefab, null, true);

                    thing.transform.position = genericObject.position;
                }
                else
                {
                    try
                    {
                        // instantilize with room as parent
                        GameObject thing = Instantiate(prefab, roomGameObjects[uniqueID].transform, true);

                        thing.transform.position = genericObject.position;
                    }
                    catch
                    {
                        LogToFile.Log("FIXME: tried to get room " + uniqueID.ToString() + "/" + roomGameObjects.Count.ToString());
                    }
                }
            }
        }

        NukeRoomChildren(false, true, true);
    }

    #endregion
}
