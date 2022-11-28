/*
 * Programmers: Jack Kennedy
 * Purpose: saves data to JSON files
 * Inputs: player stats, existing JSON files
 * Outputs: saves to JSON files
 */

using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine;

public class SqliteManager : MonoBehaviour
{
    #region Variables

    SavesManager savesManager; // variable for the saves manager
    healthManager healthManager; // our health manager
    GameObject player; // our player

    #endregion

    #region Default Methods

    private void Awake()
    {
        savesManager = FindObjectOfType<SavesManager>(); //assign the saves manager
        player = GameObject.Find("player"); // grab our player
        healthManager = player.GetComponent<healthManager>(); // get our health manager
    }

    #endregion

    #region Custom Methods

    public void Read(string saveName)
    {
        // get the save file
        string saveFile = Application.persistentDataPath + "/Saves/" + saveName + ".json";

        // JSON data from our file we get by reading the entire file
        string json = File.ReadAllText(saveFile);

        // by default we'll assume we can load this save
        bool canLoad = true;

        try // we'll try to load this save as a new save, if the objects dont match it will throw an exception
        {
            // this is just a dummy so i use _
            Save _ = JsonUtility.FromJson<Save>(json);
        }
        catch // if we get to this block, it means there was an unrecoverable version mismatch.
        {
            // record that we CANNOT load this save
            canLoad = false;
        }

        if (canLoad) // this means we've passed our checks
        {
            savesManager.currentSave = JsonUtility.FromJson<Save>(json);
        }
        else
        {
            // this will later be changed to proper code where we tell the user this
            
            Debug.Log("Save version mismatch, the save is considered unloadable.");
        }
    }

    public void Write(string saveName)
    {
        // this is the path to the file using persistentDataPath
        string saveFile = Application.persistentDataPath + "/Saves/" + saveName + ".json";

        // these are values we dont update before we start saving.
        // for example, we set the playerx here since its not like we would want to update that every frame
        savesManager.currentSave.playerx = player.transform.position.x;
        savesManager.currentSave.playery = player.transform.position.y;
        savesManager.currentSave.playerHealth = healthManager.health;
        savesManager.currentSave.playerMaxHealth = healthManager.maxHealth;

        // this is our actual json data that we've made for the file
        string json = JsonUtility.ToJson(savesManager.currentSave);

        // we'll write all this json data to the file
        File.WriteAllText(saveFile, json);
    }

    #endregion
}