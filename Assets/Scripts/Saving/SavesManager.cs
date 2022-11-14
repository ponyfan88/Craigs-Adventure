/* 
 * Programmers: Jack Kennedy
 * Purpose: Manages saves
 * Inputs: call to save/load
 * Outputs: save files
 */

using System.Collections.Generic; // will be used later
using UnityEngine;
using System.IO;
using save;

public class SavesManager : MonoBehaviour
{
    #region Variables
    
    public int savesCount = 0; // we'll set this later
    public bool loadingSave = false;
    public int seed = 1;
    public Save currentSave = new Save();
    
    SqliteManager sqliteManager;
    Pause pause;

    #endregion

    #region Default Methods

    private void Start()
    {
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
        sqliteManager = GetComponent<SqliteManager>();
        pause = FindObjectOfType<Pause>();

        DirectoryInfo savesFolder = new DirectoryInfo(Application.dataPath + "//saves//"); // get the saves folder and scan it for .jsave files
        
        try // we are going to try this, since the directory might not exist
        {
            FileInfo[] saves = savesFolder.GetFiles("*.sqlite");
            savesCount = saves.Length; // get the number of saves
        }
        catch // in this case, the directory doesnt exist. we'll make the directory for future saves
        {
            savesFolder.Create(); // make the "saves" directory.
            savesCount = 0; // since we just made the directory, it's empty.
        }
    }

    #endregion

    #region Custom Methods

    public void saveScene(string saveName = null) // called by pause
    {
        if (string.IsNullOrEmpty(saveName) || string.IsNullOrWhiteSpace(saveName) || saveName == "\u200b")
        {
            saveName = "my save";
        }

        // to explain these next few lines: if a save exists with the same name as one we are deleting, delete that save.

        DirectoryInfo savesFolder = new DirectoryInfo(Application.dataPath + "//saves//"); // get the saves folder and scan it for .jsave files
        FileInfo[] saves = savesFolder.GetFiles(saveName + ".sqlite");

        if (saves.Length > 0)
        {
            LogToFile.Log("save \"" + saveName + ".sqlite\" existed. deleting save.");
            try
            {
                saves[0].Delete();
                LogToFile.Log("deleted file.");
            }
            catch
            {
                LogToFile.Log("we cannot delete the file, but thats okay since its the same save version.");
            }
        }

        sqliteManager.Write(saveName);
    }

    public void loadScene(string saveName = null) // called by pause
    {
        if (string.IsNullOrEmpty(saveName) || string.IsNullOrWhiteSpace(saveName) || saveName == "\u200b")
        {
            saveName = "my save";
        }

        // run the read function inside sqlite manager
        sqliteManager.Read(saveName);

        loadingSave = true; // we are now loading the save

        pause.RestartGame(); // restart our game
    }

    #endregion
}
