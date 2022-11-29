/* 
 * Programmers: Jack Kennedy
 * Purpose: Manage the main menu UI
 * Inputs: player pressing buttons
 * Outputs: the main menu
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variables

    // import sound manager
    SoundManager soundManager;

    // import saves manager
    SavesManager savesManager;

    #endregion

    #region  Default Methods
    
    void Awake()
    {
        // we need to set this since we dont destroy this on load
        FloorManager.floor = 1; // we are on floor 1

        // we will set ourselves to be paused
        Pause.paused = true;

        soundManager = FindObjectOfType<SoundManager>(); // make it a variable so we can use it
        soundManager.Play("Main Menu"); // play the main menu theme

        savesManager = FindObjectOfType<SavesManager>(); // assign the saves manager
        savesManager.loadingSave = false; // we are no longer loading a save.
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: load the game
     * inputs: none
     * outputs: loads the game scene and fixes variables for such
     */
    public void StartGame()
    {
        LogToFile.Log("started a new game at " + Time.time.ToString());

        // the game has no longer ended
        Pause.ended = false;
        
        savesManager = FindObjectOfType<SavesManager>(); // assign the saves manager
        savesManager.loadingSave = false; // we are no longer loading a save.

        SceneManager.LoadScene("Scenes/Game"); // load the game
    }

    /*
     * purpose: closes craigs adventure
     * inputs: none
     * outputs: none
     */
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
