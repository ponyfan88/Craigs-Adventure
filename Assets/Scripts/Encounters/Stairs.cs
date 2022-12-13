/* 
 * Programmers: Jack Kennedy and Christopher Kowalewski 
 * Purpose: script that runs for each room; does various things
 * Inputs: collisions to the room
 * Outputs: various variables to other script files such as map.cs
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{

    #region Variables

    CapsuleCollider2D playerHitbox; // the player's capsule collider

    SavesManager savesManager;

    Pause pause;

    #endregion

    #region Default Methods

    private void Awake()
    {
        playerHitbox = GameObject.Find("player").GetComponent<CapsuleCollider2D>(); // get the player as a CapsuleCollider2D
        // this helps us detect if it was specifically the player, rather than their attack or an item.

        savesManager = FindObjectOfType<SavesManager>();

        pause = FindObjectOfType<Pause>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerHitbox) // the player has entered the room
        {
            FloorManager.floor++; // we are now one floor up
            FloorManager.loadSaveOverride = true; // override our save
            savesManager.loadingSave = false; // we are no longer loading a save
            if (FloorManager.floor >= 4)
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