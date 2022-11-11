/*Programmer: Christopher Kowalewski
 * Purpose: Spawn enemies when a room is entered
 * Inputs: Room.cs Calls for enemies to spawn
 * Outputs: Instantiates enemies
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] enemies;
    private int e;

    #endregion

    #region Custom Methods

    //Spawning an enemy when called - no parameters
    public void Spawn()
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        e = Random.Range(0, enemies.Length);
        Instantiate<GameObject>(enemies[e], transform.position, enemies[e].transform.rotation);
    }

    //overload - Parent transform provided
    public void Spawn(Transform parent)
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        e = Random.Range(0, enemies.Length);
        Instantiate<GameObject>(enemies[e], transform.position, enemies[e].transform.rotation, parent);
    }

    #endregion
}
