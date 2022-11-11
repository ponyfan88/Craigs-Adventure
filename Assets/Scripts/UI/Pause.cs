/* 
 * Programmers: Jack Kennedy
 * Purpose: Manage the pause menu
 * Inputs: when the game is paused
 * Outputs: pauses the game and displays pause menu
 */

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    #region Variables

    public static bool paused = true; // if we are paused // initialize as true since loading the game pauses the game
    public static bool ended = false; // if we have ended the game

    public GameObject pauseUI; // the pause menu
    public GameObject deathUI; // the death menu
    public GameObject loadingUI; // the screen to show while loading
    public GameObject emailUI; // the email menu

    public GameObject enemies; // our enemies

    public GameObject saveAsUI; // our save as menu
    public GameObject loadAsUI; // our load as menu

    SoundManager soundManager; // our sound manager
    SavesManager savesManager; // our saves manager
    FloorManager floorManager; // our floor manager

    public TextMeshProUGUI saveNameInput; // the input field for our save name in the saving menu
    public TextMeshProUGUI loadNameInput; // the input field for our save name in the loading menu

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>(); // need so that we can play sounds
        savesManager = FindObjectOfType<SavesManager>(); // need so that we can do saving commands
        floorManager = FindObjectOfType<FloorManager>(); // need so that we can do things proper

        if (floorManager.floor >= 4)
        {
            ResumeGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !ended) // if we press pause and we havent ended the game
        {
            if (paused) // if we are currently paused, unpause. otherwise, pause the game
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    #endregion

    #region Custom Methods
    
    /*
     * purpose: enables the "save as" menu
     * inputs: none
     * outputs: shows/hides ui gameobjects
     */
    public void SaveAs() // OUR SAVE AS MENU
    {
        pauseUI.SetActive(false); // make sure we dont show the pause menu
        
        saveAsUI.SetActive(true); // show our save as menu
    }

    /*
     * purpose: runs a method in our savesmanager to save our save file
     * inputs: as much as there are no parameters, it does grab the save name from an input box
     * outputs: tells the saves manager that we need to save
     */
    public void runSave() // ACTUALLY SAVE THE GAME
    {
        saveAsUI.SetActive(false); // hide the save as ui

        // we input the save name input
        savesManager.saveScene(saveNameInput.text); // have our saves manager save the current scene

        ResumeGame(); // resume the game
    }

    /*
     * purpose: enables the "load as" menu
     * inputs: none
     * outputs: shows/hides ui gameobjects
     */
    public void LoadAs() // OUR LOAD WHAT SAVE FILE MENU
    {
        pauseUI.SetActive(false); // make sure we dont show the pause menu
        
        loadAsUI.SetActive(true); // show our save as menu
    }

    /*
     * purpose: runs a method in our savesmanager to load our save file
     * inputs: as much as there are no parameters, it does grab the save name from an input box
     * outputs: tells the saves manager that we need to load
     */
    public void runLoad() // ACTUALLY LOAD THE SAVE
    {
        loadAsUI.SetActive(false); // hide our loadas menu

        // we input the save name input
        savesManager.loadScene(loadNameInput.text); // have our saves manager save the current scene
    }

    /*
     * purpose: resumes the game
     * inputs: none
     * outputs: sets the time scale to 1 (resuming the game), shows/hides ui objects
     */
    public void ResumeGame() // called when we unpause
    {
        pauseUI.SetActive(false); // hide our pause menu
        
        deathUI.SetActive(false); // hide our death menu
        
        paused = false; // we are no longer paused, make sure every other script knows that
        
        Time.timeScale = 1f; // set our timescale to 1, resuming the game
    }

    /*
     * purpose: pauses the game
     * inputs: none
     * outputs: plays pause music, slows down the game to 0, and shows the pause menu
     */
    public void PauseGame() // called when we press pause
    {
        pauseUI.SetActive(true); // show our pause menu
        
        paused = true; // we are now paused, this boolean will show other scripts that
        
        Time.timeScale = 0f; // set our timescale to 0, effectively slowing down the game so much that it is hidden
        
        soundManager.Play("Pause Music"); // play our pause music
    }

    /*
     * purpose: ends the game
     * inputs: none
     * outputs: plays death music, slows down the game to 0, and shows the death menu
     */
    public void EndGame() // called on player death
    {
        pauseUI.SetActive(false); // hide our pause menu
        
        deathUI.SetActive(true); // hide our death menu
        
        paused = true; // we have paused the game
        
        ended = true; // we have also ended the game
        
        Time.timeScale = 0f; // slow down our game to 0% speed
        
        savesManager.loadingSave = false; // we are not going to be loading a save
        
        soundManager.Play("Death Music"); // play the death theme
    }

    /*
     * purpose: leaves the game
     * inputs: none
     * outputs: loads the main menu and does practically what ResumeGame() does
     */
    public void LeaveGame() // called when pressing the main menu button
    {
        Time.timeScale = 1f; // resume the game to full speed
        
        SceneManager.LoadScene("Scenes/MainMenu"); // load the main menu
    }

    /*
     * purpose: restarts the game
     * inputs: none
     * outputs: loads the main menu and does practically what ResumeGame() does
     */
    public void RestartGame() // called when we respawn
    {
        Time.timeScale = 1f; // set our timescale to 1, resuming the game
        
        savesManager.loadingSave = false; // we dont want to load a save after we reload the game
        
        // since loadingsave is kept after we reload we can actually save this
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload the current active scene
    }

    /*
     * purpose: quits the game
     * inputs: none
     * outputs: literally quits the game
     */
    public void QuitGame() // called when we press quit game
    {
        Application.Quit(); // quits the game (only works after properly compiling the program)
    }

    /*
     * purpose: show other scripts and the user that we are loading
     * inputs: none
     * outputs: pause the game, disable the enemies, show the loading menu
     */
    public void StartLoading() // called at the start of the program
    {
        loadingUI.SetActive(true); // show our loading menu
        
        enemies.SetActive(false); // make sure our enemies arent active

        paused = true; // consider us paused
    }

    /*
     * purpose: show other scripts and the user that we are done loading
     * inputs: none
     * outputs: unpause the game, enable the enemies, hide the loading menu
     */
    public void EndLoading() // called during nav mesh generation
    {
        loadingUI.SetActive(false); // hide our loading menu

        enemies.SetActive(true); // make our enemies active

        ResumeGame(); // we are resumed! this does many things, including unpausing the game
    }

    /*
     * purpose: pause our game and show our email menu
     * inputs: none
     * outputs: consider us paused, show the email menu, set the timescale to 0
     */
    public void PauseCrashed() // called when we pause the game on an exception
    {
        paused = true; // we are paused

        emailUI.SetActive(true); // show our email menu

        Time.timeScale = 0f; // slow down our game to 0% speed
    }

    /*
     * purpose: resumes the game after we send an email
     * inputs: none
     * outputs: consider us unpaused, hide the email menu, set the timescale to 1
     */
    public void ResumeCrashed() // called after sending an email
    {
        paused = false; // we are unpaused

        emailUI.SetActive(false); // hide our email menu

        Time.timeScale = 1f; // set our timescale to 1, resuming the game
    }

    #endregion
}
