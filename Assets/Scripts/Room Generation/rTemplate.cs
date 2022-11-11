/*
 * Programmers: Christopher Kowalewski
 * Purpose: Stores prefab references for scripts to access.
 * Inputs: prefabs from prefab folder in unity/the overall project.
 * Outputs: Used by rGen and globalGen to pull prefabs for instantiating (cloning)
 */

using UnityEngine;

public class rTemplate : MonoBehaviour
{
    public GameObject[] tRooms; //storage/reference for rooms with a top/north-facing wall
    public GameObject[] bRooms; //storage/reference for rooms with a bottom/south-facing wall
    public GameObject[] lRooms; //storage/reference for rooms with a left-facing wall
    public GameObject[] rRooms; //storage/reference for rooms with a right-facing wall

    public GameObject[] mutliRooms;
    public GameObject[] testSpawns;

    public GameObject[] Walls; //storage/reference for extra walls (to block off doors that lead to unspawned rooms and such)
    public GameObject[] Boss; //end rooms (currently only staircase rooms)
}
