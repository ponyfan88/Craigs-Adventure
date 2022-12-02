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

        // deal with the saves directory existing or not
        if (!Directory.Exists(Application.streamingAssetsPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/");
        }
    }
    
    #endregion

    #region Custom Methods

    public void Read(string saveName)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase(saveName);  // connect to our database
        IDbCommand dbCommand = dbConnection.CreateCommand(); // create a command
        dbCommand.CommandText = "SELECT * FROM Save"; // make that command to read all the data
        IDataReader dataReader = dbCommand.ExecuteReader(); // execute our command (to read all data)

        dataReader.Read(); // open the file

        savesManager.currentSave.seed = dataReader.GetInt32(0); // our seed has the index 1

        dbConnection.Close(); // just like logging we close

        // now, do the player info

        dbConnection = CreateAndOpenDatabase(saveName);  // connect to our database
        dbCommand = dbConnection.CreateCommand(); // create a command
        dbCommand.CommandText = "SELECT * FROM Player"; // make that command to read all the data
        dataReader = dbCommand.ExecuteReader(); // execute our command (to read all data)

        dataReader.Read(); // open the file

        savesManager.currentSave.playerMaxHealth = dataReader.GetInt32(1); // our maxhealth has the index 3

        savesManager.currentSave.playerHealth = dataReader.GetInt32(0); // our health has the index 2

        savesManager.currentSave.playerx = dataReader.GetFloat(2);

        savesManager.currentSave.playery = dataReader.GetFloat(3);

        dbConnection.Close(); // just like logging we close
    }

    public void Write(string saveName)
    {
        // if we dont have a table yet, insert a save id and seed
        IDbConnection dbConnection = CreateAndOpenDatabase(saveName); 
        IDbCommand dbCommand = dbConnection.CreateCommand(); // just like above we create a command to do this
        
        dbCommand.CommandText = "INSERT OR REPLACE INTO Save (seed) VALUES (" + savesManager.currentSave.seed + ")";
        dbCommand.ExecuteNonQuery(); // execute said command

        dbCommand = dbConnection.CreateCommand();

        player = GameObject.Find("player");

        dbCommand.CommandText = "INSERT OR REPLACE INTO Player (health, maxhealth, playerx, playery) VALUES ("  + healthManager.health + ", " + healthManager.maxHealth + ", " + player.transform.position.x + ", " + player.transform.position.y + ")"; // 10
        dbCommand.ExecuteNonQuery(); // execute said command

        dbConnection.Close(); // just like logging we close
    }

    public IDbConnection CreateAndOpenDatabase(string saveName)
    {
        string dataBase = "URI=file:" + Application.streamingAssetsPath + "/Saves/" + saveName + ".sqlite"; // we call it saves.sqlite
        IDbConnection dbConnection = new SqliteConnection(dataBase); // connection to our database
        dbConnection.Open(); // open our database

        IDbCommand dbCommand = dbConnection.CreateCommand();
        // create a table if it doesnt exist yet
        // it has an id for each save, as well as a seed
        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS Save (seed INTEGER )";
        dbCommand.ExecuteReader();

        dbCommand = dbConnection.CreateCommand();

        player = GameObject.Find("player");

        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS Player (health INTEGER, maxhealth INTEGER, playerx FLOAT, playery FLOAT )";
        dbCommand.ExecuteReader();

        return dbConnection;
    }

    #endregion
}