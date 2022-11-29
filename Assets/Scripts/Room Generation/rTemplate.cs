/*
 * Programmers: Christopher Kowalewski
 * Purpose: Stores prefab references for scripts to access.
 * Inputs: prefabs from prefab folder in unity/the overall project.
 * Outputs: Used by rGen and globalGen to pull prefabs for instantiating (cloning)
 */

using System.Collections.Generic;
using UnityEngine;

public class rTemplate : MonoBehaviour
{
    [SerializeField] private GameObject[] topRooms; //default state of trooms
    [SerializeField] private GameObject[] botRooms; //default state of brooms
    [SerializeField] private GameObject[] lefRooms; //default state of lrooms
    [SerializeField] private GameObject[] rigRooms; //default state of rrooms

    public List<GameObject> tRooms = new List<GameObject>(); //storage/reference for rooms with a top/north-facing wall
    public List<GameObject> bRooms = new List<GameObject>(); //storage/reference for rooms with a bottom/south-facing wall
    public List<GameObject> lRooms = new List<GameObject>(); //storage/reference for rooms with a left-facing wall
    public List<GameObject> rRooms = new List<GameObject>(); //storage/reference for rooms with a right-facing wall

    public GameObject[] Walls; //storage/reference for extra walls (to block off doors that lead to unspawned rooms and such)
    public GameObject[] Boss; //end rooms (currently only staircase rooms)

    public void resetTemplate()
    {
        //reset lists to default values (called between floors)
    }
}
