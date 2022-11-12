/*Programmer: Christopher Kowalewski
 * Purpose: Spawn enemies when a room is entered
 * Inputs: Room.cs Calls for enemies to spawn
 * Outputs: Instantiates from enemiesTemplate
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    #region Variables

    public GameObject enemy;

    [SerializeField] private GameObject[] enemiesTemplate;
    private int e;

    #endregion

    #region Custom Methods

    //Spawning an enemy when called - no parameters
    public void Spawn()
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        e = Random.Range(0, enemiesTemplate.Length);
        enemy = Instantiate<GameObject>(enemiesTemplate[e], transform.position, enemiesTemplate[e].transform.rotation);
    }

    //overload - Parent transform provided
    public void Spawn(Transform parent)
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        e = Random.Range(0, enemiesTemplate.Length);
        enemy = Instantiate<GameObject>(enemiesTemplate[e], transform.position, enemiesTemplate[e].transform.rotation, parent);
    }

    #endregion
}
