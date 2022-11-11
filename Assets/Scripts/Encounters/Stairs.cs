/* 
 * Programmers: Jack Kennedy and Christopher Kowalewski 
 * Purpose: script that runs for each room; does various things
 * Inputs: collisions to the room
 * Outputs: various variables to other script files such as map.cs
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{

    #region Variables

    CapsuleCollider2D playerHitbox; // the player's capsule collider

    SavesManager savesManager;

    FloorManager floorManager;

    Pause pause;

    #endregion

    #region Default Methods

    private void Awake()
    {
        playerHitbox = GameObject.Find("player").GetComponent<CapsuleCollider2D>(); // get the player as a CapsuleCollider2D
        // this helps us detect if it was specifically the player, rather than their attack or an item.

        savesManager = FindObjectOfType<SavesManager>();

        floorManager = FindObjectOfType<FloorManager>();

        pause = FindObjectOfType<Pause>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerHitbox) // the player has entered the room
        {
            floorManager.floor++; // we are now one floor up
            floorManager.loadSaveOverride = true; // override our save
            if (floorManager.floor >= 4)
            {
                SceneManager.LoadScene("Scenes/BossLevel");
            }
            else
            {
                pause.RestartGame();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // do nothing
    }

    #endregion
}