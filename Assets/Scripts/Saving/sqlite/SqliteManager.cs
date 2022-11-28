/*
 * Programmers: Jack Kennedy
 * Purpose: to interact with sqlite
 * Inputs: calls to methods here
 * Outputs: saves to our database
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

        // its as easy as this, we just have FromJson<Save> do all the heavy listing,
        // taking our JSON data and converting it to type Save
        savesManager.currentSave = JsonUtility.FromJson<Save>(json);
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