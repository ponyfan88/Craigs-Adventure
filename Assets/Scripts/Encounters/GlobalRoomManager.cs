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
    
    [NonSerialized] public bool inEncounter = false, inRoom = false;
    public void EnterEncounter()
    {
        inEncounter = true;
    }
    public void ExitEncounter()
    {
        inEncounter = false;
    }
    public void EnterRoom()
    {
        inRoom = true;
    }
    public void ExitRoom()
    {
        inRoom = false;
    }
}
