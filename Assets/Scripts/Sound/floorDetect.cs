/*
 * programmers: Christopher Kowalewski
 * purpose: Detects the type of floor tile to change walk sound
 * inputs: Tilemap data
 * outputs: Surface int; what it does is describe what kind of tile you've stepped on (not currently used in other implementations)
 */

using UnityEngine;
using UnityEngine.Tilemaps;

public class floorDetect : MonoBehaviour
{
    #region Variables

    public Tilemap tileMap;
    public int surface = 0;

    [SerializeField] private controller player; //player controller
    [SerializeField] private Grid grid; //room grid

    [SerializeField] private Sprite[] woods;
    [SerializeField] private Sprite[] stones;

    private Vector3 playerPos; //player's position
    private Vector3Int tileCoord; //the whole number position the player is standing on
    private Sprite surfaceSprite;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position; //get and store player position
        tileCoord = grid.WorldToCell(playerPos); //convert it to a whole (int) value
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position; //get and store player position
        tileCoord = grid.WorldToCell(playerPos); //convert it to a whole value

        Debug.Log(tileMap.GetSprite(tileCoord)); //Wooden_Floor_5 and Wooden_Floor_17 are stone, everything else is wood.
        surfaceSprite = tileMap.GetSprite(tileCoord); //store the sprite

        bool found = false;

        for (int i = 0; i < stones.Length; ++i)
        {
            if (surfaceSprite == stones[i])
            {
                found = true;
                surface = 2;
            }
        }
        
        if (!found)
        {
            for (int i = 0; i < woods.Length; ++i)
            {
                if (surfaceSprite == woods[i])
                {
                    found = true;
                    surface = 1;
                }
            }
        }
    }

    #endregion
}
