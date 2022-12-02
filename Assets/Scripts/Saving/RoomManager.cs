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

    // Start is called before the first frame update
    void Start()
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
                rooms[i] = savesManager.currentSave.rooms[i];    
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
    public void NukeRoomChildren()
    {
        Time.timeScale = 0f; // set our timescale to 0, stopping the game.

        rooms = FindObjectsOfType<Room>();

        // create a list of the rooms themselves
        List<GameObject> roomGameObjects = new List<GameObject>();

        // fill that list with each rooms parent
        foreach (Room room in rooms)
        {
            // lowercase g
            roomGameObjects.Add(room.gameObject);
        }

        foreach (GameObject room in roomGameObjects)
        {
            foreach (Transform child in room.transform)
            {
                Destroy(child.parent); // ok, so this is actually the gameobject AFAIK
            }
        }

    }

    #endregion
}
