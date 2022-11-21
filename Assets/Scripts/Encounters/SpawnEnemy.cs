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
    private int index;

    #endregion

    #region Custom Methods

    //Spawning an enemy when called - no parameters
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void Spawn()
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        index = Random.Range(0, enemiesTemplate.Length);
        enemy = Instantiate<GameObject>(enemiesTemplate[index], transform.position, enemiesTemplate[index].transform.rotation);
    }

    //overload - Parent transform provided
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void Spawn(Transform parent)
    {
        //possibly play spawning animation

        //instantiate enemy at this position
        index = Random.Range(0, enemiesTemplate.Length);
        enemy = Instantiate<GameObject>(enemiesTemplate[index], transform.position, enemiesTemplate[index].transform.rotation, parent);
    }

    #endregion
}
