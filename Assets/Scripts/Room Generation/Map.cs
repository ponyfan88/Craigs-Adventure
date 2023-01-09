/*
 * Programmers: Christopher Kowalewski
 * Purpose: Provides a map for the player to view through the UI
 * Inputs: ExpandMap button, Room.cs adds rooms to discovered rooms list.
 * Outputs: updates objects seen by a camera sent to a rendertexture used by the map UI
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Variables

    [SerializeField] public List<GameObject> discovered = new List<GameObject>(); // list of rooms craig entered (from Room.cs calls)

    [SerializeField] private GameObject mapUI; // our map ui
    [SerializeField] private RenderTexture mapTexture; //render texture that takes the camera and helps display the map
    [SerializeField] private GameObject[] mapTemplate; // a template of map object prefabs (instantiated to show on camera aka map)

    [NonSerialized] private List<GameObject> map = new List<GameObject>(); // stores a list of actively mapped rooms

    [NonSerialized] private GameObject player; // stores reference to player object

    [NonSerialized] private bool maximized = false; // boolean for if our map is maximized or not
    [NonSerialized] private RectTransform UITransform; //rectTransform of the mapUI object

    [SerializeField] private GameObject[] wallTemplate; // wall templates to spawn

    [NonSerialized] private SavesManager savesManager; // saves manager

    #endregion

    #region Default Methods

    private void Awake()
    {
        player = GameObject.Find("player"); // grab our player
        UITransform = mapUI.GetComponent<RectTransform>();

        savesManager = FindObjectOfType<SavesManager>();
    }

    private void FixedUpdate()
    {
        if (discovered.Count > map.Count) // if we've discovered a new room
        {
            // clear our map since we've discovered another room
            ClearMap(); // clear our current minimap so that when we generate a new map it doesnt layer over / create unnecessary objects

            // we now add back every room, including the one we just discovered
            foreach (GameObject room in discovered) // for every discovered room
            {
                // get our current room's type (in this case we call it the room's "template"
                GameObject roomTemplate = mapTemplate[room.GetComponentInChildren<Room>().roomType];

                GameObject mapRoom = Instantiate(roomTemplate, new Vector3(room.transform.position.x / 100, room.transform.position.y / 100, 10), roomTemplate.transform.rotation, gameObject.transform);

                // add that room
                map.Add(mapRoom);

                Wall[] walls = room.GetComponentsInChildren<Wall>();

                foreach (Wall wall in walls)
                {
                    /*
                     * PLEASE
                     *   2
                     * 1 + 3
                     *   4
                     */

                    Instantiate(wallTemplate[wall.direction - 1], mapRoom.transform);
                }

            }
        }

        if (maximized) // if we are holding down the key for expanding the map
        {
            UITransform.sizeDelta = new Vector2(800, 800); // make it large
            UITransform.anchoredPosition = new Vector2(-400, 0); //center it

            mapTexture = new RenderTexture(800, 800, 24);
        }
        else // assuming the button isnt being held down
        {
            mapUI.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200); // make it small
            UITransform.anchoredPosition = Vector2.zero; //position at corner

            mapTexture = new RenderTexture(200, 200, 24);
        }
    }

    private void Update()
    {
        // get our button for if we've expanded the map
        // we call this in update as fixedupdate tends to "skip over" frames
        maximized = Input.GetButton("ExpandMap");

        // every frame update our camera's position to follow the player on the map
        gameObject.GetComponentInChildren<Camera>().gameObject.transform.position = new Vector3(player.transform.position.x / 100, player.transform.position.y / 100, 5);
    }

    #endregion

    #region Custom Methods

    private void ClearMap() // method that clears
    {
        // for every room that our minimap currently "has"
        foreach (GameObject room in map)
        {
            // destroy the room from the scene
            Destroy(room);
        }
        // clear the list
        map.Clear();
    }

    #endregion
}
