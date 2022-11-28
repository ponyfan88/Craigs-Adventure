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
        player = GameObject.Find("player"); // grab out player
        healthManager = player.GetComponent<healthManager>(); // get our health manager
    }

    #endregion

    #region Custom Methods

    public void Read(string saveName)
    {
        // get the save file
        string saveFile = Application.persistentDataPath + "/Saves/" + saveName + ".json";

        savesManager.currentSave = JsonUtility.FromJson<Save>(File.ReadAllText(saveFile));
    }

    public void Write(string saveName)
    {
        string saveFile = Application.persistentDataPath + "/Saves/" + saveName + ".json";

        savesManager.currentSave.playerx = player.transform.position.x;
        savesManager.currentSave.playery = player.transform.position.y;
        savesManager.currentSave.playerHealth = healthManager.health;
        savesManager.currentSave.playerMaxHealth = healthManager.maxHealth;

        string json = JsonUtility.ToJson(savesManager.currentSave);

        File.WriteAllText(saveFile, json);
    }

    #endregion
}