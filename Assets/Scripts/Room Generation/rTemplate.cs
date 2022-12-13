/*
 * Programmers: Christopher Kowalewski
 * Purpose: Stores prefab references for scripts to access.
 * Inputs: prefabs from prefab folder in unity/the overall project.
 * Outputs: Used by rGen and globalGen to pull prefabs for instantiating (cloning)
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class rTemplate : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] topRooms; //default state of trooms
    [SerializeField] private GameObject[] botRooms; //default state of brooms
    [SerializeField] private GameObject[] lefRooms; //default state of lrooms
    [SerializeField] private GameObject[] rigRooms; //default state of rrooms

    [NonSerialized] public List<GameObject> tRooms = new List<GameObject>(); //storage/reference for rooms with a top/north-facing wall
    [NonSerialized] public List<GameObject> bRooms = new List<GameObject>(); //storage/reference for rooms with a bottom/south-facing wall
    [NonSerialized] public List<GameObject> lRooms = new List<GameObject>(); //storage/reference for rooms with a left-facing wall
    [NonSerialized] public List<GameObject> rRooms = new List<GameObject>(); //storage/reference for rooms with a right-facing wall

    public GameObject[] Walls; //storage/reference for extra walls (to block off doors that lead to unspawned rooms and such)
    public GameObject[] Boss; //end rooms (currently only staircase rooms)

    #endregion

    #region Default Methods

    private void Awake()
    {
        resetTemplate(); //reset lists to default
    }

    #endregion

    #region Custom Methods

    /*Method Purpose: Resets the list of rooms to its default state, before generation affects the list of rooms
     * Method Inputs: none, called during first frame of the scene
     * Method Outputs: Lists containing room prefab tenplates get reset to default
     */
    public void resetTemplate()
    {
        //clears any remaining rooms in the lists
        tRooms.Clear();
        bRooms.Clear();
        lRooms.Clear();
        rRooms.Clear();

        //this set of for loops goes through each item in the default room arrays and adds them back to the lists
        for (int i=0; i<topRooms.Length; ++i)
        {
            tRooms.Add(topRooms[i]);
        }
        for (int i = 0; i < botRooms.Length; ++i)
        {
            bRooms.Add(botRooms[i]);
        }
        for (int i = 0; i < lefRooms.Length; ++i)
        {
            lRooms.Add(lefRooms[i]);
        }
        for (int i = 0; i < rigRooms.Length; ++i)
        {
            rRooms.Add(rigRooms[i]);
        }
    }

    #endregion
}
