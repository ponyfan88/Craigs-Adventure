/* 
 * Programmers: Jack Kennedy
 * Purpose: Manages saves
 * Inputs: call to save/load
 * Outputs: save files
 */

using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SavesManager : MonoBehaviour
{
    #region Variables
    
    public int savesCount = 0; // we'll set this later
    public bool loadingSave = false; // we arent loading a save by default
    public int seed = 1; // our seed (initialize as 1 since thats a good, noticeable seed. it generates a sort of upside-down stairs pattern.)
    public Save currentSave = new Save(); // our save. see save.cs for this classes info. it's in the same directory.
    
    DataManager dataManager;
    ThingManager thingManager;

    Map map; //find the map script

    #endregion

    #region Default Methods

    /*
     * purpose: various setup things.
     * inputs: clones of ourselves, existing save files.
     * outputs: save count and saves folder, as well as variables needed for the custom methods to run
     */
    private void Start()
    {
        // get the thing manager so that we will be able to save items and enemies
        thingManager = FindObjectOfType<ThingManager>();

        // get every saving object
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Saving");

        // if we have more than one of them
        if (objs.Length > 1)
        {
            // DESTROY IT
            Destroy(this.gameObject);
        }

        // make sure we dont destroy it or anything like that LOLOLOLOL
        DontDestroyOnLoad(this.gameObject);

        // grab variable things
        dataManager = GetComponent<DataManager>();
        
        if (!Directory.Exists(Application.streamingAssetsPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/");

            savesCount = 0;
        }
        else
        {
            DirectoryInfo savesFolder = new DirectoryInfo(Application.streamingAssetsPath + "/Saves/"); // get the saves folder and scan it for .sqlite files
            FileInfo[] saves = savesFolder.GetFiles("*.sqlite");
            savesCount = saves.Length;
        }
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: tells the sqlite manager to save our game
     * inputs: the save name
     * outputs: see purpose
     */
    public void saveScene(string saveName = null) // called by pause
    {
        LogToFile.Log("validating save name");
        
        // here we check to make sure that our input string is totally ok to be a save name
        // "\u200b" is the default (empty) input - but we check for other ones just in case.
        // this isnt called every frame, so im not worried about any extra checks just in case
        if (string.IsNullOrEmpty(saveName) || string.IsNullOrWhiteSpace(saveName) || saveName == "\u200b")
        {
            saveName = "my save";
        }

        // to explain these next few lines: if a save exists with the same name as one we are deleting, delete that save.

        if (!Directory.Exists(Application.streamingAssetsPath + "/Saves/"))
        {
            LogToFile.Log("saves directory didnt exist; gonna make dat");
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/");
            LogToFile.Log("created saves directory!");
        }

        LogToFile.Log("getting directory info");
        DirectoryInfo savesFolder = new DirectoryInfo(Application.streamingAssetsPath + "/Saves/"); // get the saves folder and scan it for .jsave files

        LogToFile.Log("finding save files of matching names");
        FileInfo[] saves = savesFolder.GetFiles(saveName + ".sqlite");

        if (saves.Length > 0) // this means that we found a file with the same name
        {
            LogToFile.Log("save \"" + saveName + ".sqlite\" existed. deleting save.");
            
            try // try to delete the file. this only works if we havent saved the file this session
            {
                saves[0].Delete();
                LogToFile.Log("deleted file.");
            }
            catch // this throws if we've already saved a file by this name this session
            {
                // to explain why this is okay, this means our sqlite manager will just open the existing save and write over the data.
                // so why check in the first place? well, above actually is for OUTDATED saves, and as such we'd potentially be trying to find tables that dont exist - causing a crash
                LogToFile.Log("we cannot delete the file, but thats okay since its the same save version.");
            }
        }

        // save our active items and enemies

        LogToFile.Log("calling thingmanager");
        thingManager.NukeRoomChildren(true, false);

        LogToFile.Log("finding map");
        map = FindObjectOfType<Map>();

        LogToFile.Log("saving discovered rooms");
        foreach (GameObject discoveredRoom in map.discovered)
        {
            int uniqueID = discoveredRoom.GetComponentInChildren<Room>().uniqueID;

            currentSave.discoveredRoomIDs.Add(uniqueID);
            LogToFile.Log("saved room " + uniqueID);
        }

        LogToFile.Log("writing save.");
        dataManager.Write(saveName);
    }

    /*
     * purpose: tells the sqlite manager to load our game
     * inputs: the save name
     * outputs: see purpose
     */
    public void loadScene(string saveName = null) // called by pause
    {
        if (string.IsNullOrEmpty(saveName) || string.IsNullOrWhiteSpace(saveName) || saveName == "\u200b")
        {
            saveName = "my save";
        }

        // run the read function inside sqlite manager
        dataManager.Read(saveName);

        loadingSave = true; // we are now loading the save

        // loading save is saved between restarts, so its okay to restart our game
        Time.timeScale = 1f; // set our timescale to 1, resuming the game

        // since loadingsave is kept after we reload we can actually save this
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload the current active scene
    }

    #endregion
}
