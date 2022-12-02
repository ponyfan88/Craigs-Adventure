/*
 * Programmer: Xander Mooney
 * Purpose: generalize the conditions of rooms as to allow other scripts to use
 * Inputs: room conditions
 * Ouputs: what state the game is in
 */
using System;
using UnityEngine;

public class GlobalRoomManager : MonoBehaviour
{
    #region Variables

    [NonSerialized] public bool inEncounter = false, inRoom = false;

    #endregion
    #region Custom Methods
    /*
     * Purpose: Keep track of when an encounter is entered
     * Inputs: rooms from room.cs detecting when an encounter starts
     * Outputs: tells other scripts that an encounter has started
    */
    public void EnterEncounter()
    {
        inEncounter = true;
    }
    /*
     * Purpose: Keep track of when an encounter is exited
     * Inputs: rooms from room.cs detecting when an encounter ends
     * Outputs: tells other scripts that an encounter has ended
    */
    public void ExitEncounter()
    {
        inEncounter = false;
    }
    /*
     * Purpose: Keep track of when a room is entered
     * Inputs: rooms from room.cs detecting when its been entered
     * Outputs: tells other scripts that a room has been entered
    */
    public void EnterRoom()
    {
        inRoom = true;
    }
    /*
     * Purpose: Keep track of when a room is exited
     * Inputs: rooms from room.cs detecting when its been exited
     * Outputs: tells other scripts that a room has been exited
    */
    public void ExitRoom()
    {
        inRoom = false;
    }

    #endregion
}
