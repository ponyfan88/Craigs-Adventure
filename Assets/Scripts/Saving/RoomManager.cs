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
}
