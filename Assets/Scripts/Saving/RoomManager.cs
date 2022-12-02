using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
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
    public void NukeRoomChildren(bool save = false, bool destroy = true)
    {
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

        // we will be storing items and enemies as generic objects
        List<GenericObject> genericObjects = new List<GenericObject>();
        // we dont test for saving since Visual Studio will yell at you for not putting this here

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

                        // add it to our list of game objects
                        genericObjects.Add(genericObject);
                    }

                    if (destroy)
                    {
                        Destroy(child.gameObject); // destroy the item/enemy gameobject (they'll be replaced later)
                    }
                }
            }
        }

        // if we are not just destroying objects
        if (save)
        {
            // store our generic objects in our save
            savesManager.currentSave.genericObjects = genericObjects;
        }
    }

    public void ReconstructGenericObjects()
    {
        if (savesManager.currentSave.genericObjects == null)
        {
            NukeRoomChildren(true); // save our current progress if not true
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
        
        // we will NOT be saving over our current save
        NukeRoomChildren(false, true);
        // destroy set to true since we want to sort of "clear the board" so to speak

        Debug.Log("through the loop");

        for (int i = 0; i < savesManager.currentSave.genericObjects.Count; ++i)
        {
            GenericObject genericObject = savesManager.currentSave.genericObjects[i];

            Debug.Log(genericObject);


            // THE ROOM TO FIND: roomGameObjects[genericObject.gameObject]
            // THE CHILD TO PLACE: genericObject.potentialChild

            // put the item/enemy inside the room

            // CURRENT ISSUES:

            // 1: every component is disabled by default
            // 2: item is rotated 90,0,0
            // 3: not placed in correct room

            Instantiate(genericObject.itemEnemyThing, null, true); // TODO: MIGHT NEED WORLD SPACE
        }
    }

    #endregion
}
