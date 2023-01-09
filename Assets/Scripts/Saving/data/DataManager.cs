/*
 * Programmers: Jack Kennedy
 * Purpose: to interact with Sqlite and JSON, saving files both need to load
 * Inputs: calls to methods here
 * Outputs: saves to our database
 */

using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Variables

    SavesManager savesManager; // variable for the saves manager
    healthManager healthManager; // our health manager
    GameObject player; // our player

    #endregion

    #region Structs

    private struct GenericObjectJSON
    {
        // both equal null by default
        public thingEnums.thingPrefab thingPrefab;
        public int uniqueID;
        public int health;

        public float x;
        public float y;
        public float z;
    }

    private struct JSONSave
    {
        public List<GenericObjectJSON> genericObjectsJSON;
        public List<int> discoveredRoomIDs;
    }

    #endregion

    #region Default Methods

    private void Awake()
    {
        savesManager = FindObjectOfType<SavesManager>(); //assign the saves manager
        
        try
        {
            player = GameObject.Find("player"); // grab out player
            healthManager = player.GetComponent<healthManager>(); // get our health manager
        }
        catch
        {
            LogToFile.Log("likely on the main menu, ignore this");
        }
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
        // Sqlite before JSON
        
        #region Sqlite

        IDbConnection dbConnection = CreateAndOpenDatabase(saveName);  // connect to our database
        IDbCommand dbCommand = dbConnection.CreateCommand(); // create a command
        dbCommand.CommandText = "SELECT * FROM Save"; // make that command to read all the data
        IDataReader dataReader = dbCommand.ExecuteReader(); // execute our command (to read all data)

        dataReader.Read(); // open the file

        savesManager.currentSave.seed = dataReader.GetInt32(0); // our seed has the index 0

        FloorManager.floor = dataReader.GetByte(1); // our floor has the index 1

        dbConnection.Close(); // just like logging we close

        // now, do the player info

        dbConnection = CreateAndOpenDatabase(saveName);  // connect to our database
        dbCommand = dbConnection.CreateCommand(); // create a command
        dbCommand.CommandText = "SELECT * FROM Player"; // make that command to read all the data
        dataReader = dbCommand.ExecuteReader(); // execute our command (to read all data)

        dataReader.Read(); // open the file

        savesManager.currentSave.playerMaxHealth = dataReader.GetInt32(1); // our maxhealth has the index 1

        savesManager.currentSave.playerHealth = dataReader.GetInt32(0); // our health has the index 0

        savesManager.currentSave.playerx = dataReader.GetFloat(2); // our playerx has the index 2

        savesManager.currentSave.playery = dataReader.GetFloat(3); // our playery has the index 3

        dbConnection.Close(); // just like logging we close

        #endregion

        #region JSON

        // get the string from our file
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/Saves/" + saveName + ".json");

        JSONSave jsonSave = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONSave>(jsonString);

        savesManager.currentSave.discoveredRoomIDs = jsonSave.discoveredRoomIDs;

        List<GenericObjectJSON> jsonList = jsonSave.genericObjectsJSON;

        List<GenericObject> jsonListSwap = new List<GenericObject>();

        foreach (GenericObjectJSON genericObjectJSON in jsonList)
        {
            GenericObject genericObject = new GenericObject();

            genericObject.health = genericObjectJSON.health;

            genericObject.thingPrefab = genericObjectJSON.thingPrefab;
            genericObject.uniqueID = genericObjectJSON.uniqueID;

            genericObject.position = new Vector3(genericObjectJSON.x, genericObjectJSON.y, genericObjectJSON.z);

            jsonListSwap.Add(genericObject);
        }

        savesManager.currentSave.genericObjects = jsonListSwap;

        // convert from json to a class and load over current generic objects

        #endregion
    }

    public void Write(string saveName)
    {
        // JSON before Sqlite

        #region JSON

        // JSON cannot parse vector3s so we redo that functionality

        List<GenericObjectJSON> jsonList = new List<GenericObjectJSON>();

        foreach (GenericObject genericObject in savesManager.currentSave.genericObjects)
        {
            GenericObjectJSON genericObjectJSON = new GenericObjectJSON();

            genericObjectJSON.health = genericObject.health;

            genericObjectJSON.thingPrefab = genericObject.thingPrefab;
            genericObjectJSON.uniqueID = genericObject.uniqueID;

            genericObjectJSON.x = genericObject.position.x;
            genericObjectJSON.y = genericObject.position.y;
            genericObjectJSON.z = genericObject.position.z;

            jsonList.Add(genericObjectJSON);
        }

        JSONSave jsonSave = new JSONSave();
        jsonSave.discoveredRoomIDs = savesManager.currentSave.discoveredRoomIDs;
        jsonSave.genericObjectsJSON = jsonList;

        // convert it all to json
        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonSave);

        // write the text to our json file
        File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + saveName + ".json", jsonString);

        #endregion

        #region Sqlite

        // if we dont have a table yet, insert a save id and seed
        IDbConnection dbConnection = CreateAndOpenDatabase(saveName); 
        IDbCommand dbCommand = dbConnection.CreateCommand(); // just like above we create a command to do this
        
        dbCommand.CommandText = "INSERT OR REPLACE INTO Save (seed, floor) VALUES (" + savesManager.currentSave.seed + ", " + FloorManager.floor + ")";
        dbCommand.ExecuteNonQuery(); // execute said command

        dbCommand = dbConnection.CreateCommand();

        player = GameObject.Find("player");

        dbCommand.CommandText = "INSERT OR REPLACE INTO Player (health, max_health, player_x, player_y) VALUES ("  + healthManager.health + ", " + healthManager.maxHealth + ", " + player.transform.position.x + ", " + player.transform.position.y + ")"; // 10
        dbCommand.ExecuteNonQuery(); // execute said command

        dbConnection.Close(); // just like logging we close

        #endregion
    }

    public IDbConnection CreateAndOpenDatabase(string saveName)
    {
        string dataBase = "URI=file:" + Application.streamingAssetsPath + "/Saves/" + saveName + ".sqlite"; // we call it saves.sqlite
        IDbConnection dbConnection = new SqliteConnection(dataBase); // connection to our database
        dbConnection.Open(); // open our database

        IDbCommand dbCommand = dbConnection.CreateCommand();
        // create a table if it doesnt exist yet
        // it has an id for each save, as well as a seed
        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS Save (seed INTEGER, floor INTEGER)";
        dbCommand.ExecuteReader();

        dbCommand = dbConnection.CreateCommand();

        player = GameObject.Find("player");

        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS Player (health INTEGER, max_health INTEGER, player_x FLOAT, player_y FLOAT )";
        dbCommand.ExecuteReader();

        return dbConnection;
    }

    #endregion
}