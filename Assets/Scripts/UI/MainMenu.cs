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

    // import the floor manager
    FloorManager floorManager;

    #endregion

    #region  Default Methods
    
    void Awake()
    {
        floorManager = FindObjectOfType<FloorManager>(); // get our floor manager

        // we need to set this since we dont destroy this on load
        floorManager.floor = 1; // we are on floor 1

        // we will set ourselves to be paused
        Pause.paused = true;

        soundManager = FindObjectOfType<SoundManager>(); // make it a variable so we can use it
        soundManager.Play("Main Menu"); // play the main menu theme

        savesManager = FindObjectOfType<SavesManager>(); // assign the saves manager
        savesManager.loadingSave = false; // we are no longer loading a save.
    }

    #endregion

    #region Custom Methods

    public void StartGame()
    {
        LogToFile.Log("started a new game at " + Time.time.ToString());
        SceneManager.LoadScene("Scenes/Game"); // load the game
    }

    // pretty self explanitory
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
