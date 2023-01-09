/* 
 * Programmers: Jack Kennedy
 * Purpose: Saves data in a save class
 * Inputs: the map seed, the player health, the players max health, the player y, the player x
 * Outputs: (mirrors inputs)
 */

using System.Collections.Generic;

public class Save
{
    public int seed;
    public int playerMaxHealth;
    public int playerHealth;
    public float playerx;
    public float playery;

    public List<GenericObject> genericObjects;

    public List<int> discoveredRoomIDs = new List<int>();
}
