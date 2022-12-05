using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingManager : MonoBehaviour
{
    #region Variables

    private SavesManager savesManager;

    private Room[] rooms;

    #endregion

    #region Default Methods

    // Awake is called after the first frame update
    void Awake()
    {
        savesManager = FindObjectOfType<SavesManager>();

        // if we are loading a save
        if (savesManager.loadingSave)
        {
            // get our active rooms
            rooms = FindObjectsOfType<Room>();

            // for every room
            for (int i = 0; i < rooms.Length; ++i)
            {
                // replace that rooms data with our saved data
                //rooms[i] = savesManager.currentSave.rooms[i];    
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Custom Methods

    // nukes EVERY child in EVERY room
    public void NukeRoomChildren(bool save = false, bool destroy = true, bool loadFromSave = false)
    {
        if (!loadFromSave)
        {
            rooms = FindObjectsOfType<Room>();
            // EVERY OBJECT CALLED ROOM CHILD 
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

                    // what object type did we find?
                    Debug.Log(tag);

                    // if its an enemy or an item, thats what we're looking to save
                    if (tag == "Item" || tag == "Enemy")
                    {
                        // if we are not just destroying objects
                        if (save)
                        {
                            // we make a new GenericObject
                            GenericObject genericObject = new GenericObject();

                            // the parent is the room we're looping through
                            //genericObject.gameObject = room; // not even sure we need to save this

                            // the itemEnemyThing is the item/enemy we are looping throuhg
                            genericObject.itemEnemyThing = child.gameObject;

                            genericObject.parentUniqueID = uniqueIDs[counter];

                            // add it to our list of game objects
                            genericObjects.Add(genericObject);
                        }

                        if (destroy)
                        {
                            Destroy(child.gameObject); // destroy the item/enemy gameobject (they'll be replaced later)
                        }
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
                foreach (GenericObject genericObject in savesManager.currentSave.genericObjects)
                {
                    Destroy(genericObject.itemEnemyThing);
                }
            }
        }
    }

    public void ReconstructGenericObjects()
    {
        if (savesManager.currentSave.genericObjects == null) // owned
        {
            NukeRoomChildren(true, false); // save our current progress if not true
            // TODO: REMOVE THIS
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

        Debug.Log("through the loop");

        for (int i = 0; i < savesManager.currentSave.genericObjects.Count; ++i)
        {
            GenericObject genericObject = savesManager.currentSave.genericObjects[i];

            Debug.Log(genericObject);


            // THE ROOM TO FIND: roomGameObjects[genericObject.gameObject]
            // THE CHILD TO PLACE: genericObject.potentialChild

            // put the item/enemy inside the room

            // CURRENT ISSUES:

            // 1: every component is disabled by default // TODO:REMOVEME
            // 2: item is rotated 90,0,0 // IF WE PARENT TO ROOM THIS ISNT AN ISSUE
            // 3: not placed in correct room

            GameObject thing = Instantiate(genericObject.itemEnemyThing, roomGameObjects[genericObject.parentUniqueID - 1].transform, true); // TODO: MIGHT NEED WORLD SPACE
            thing.SetActive(true);
            thing.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        NukeRoomChildren(false, true, true);
    }

    #endregion
}
