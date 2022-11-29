/* 
 * Programmers: Jack Kennedy
 * Purpose: display boss health
 * Inputs: boss health and if we are paused
 * Outputs: a large health bar on screen
 */

using UnityEngine;

public class PointToEnd : MonoBehaviour
{
    #region Variables

    private float direction = 0f;

    private GameObject player;
    private GameObject endRoom;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player"); // grab our player
        endRoom = GameObject.FindWithTag("End Room");
    }

    // Update is called once per frame
    void Update()
    {
        direction = Mathf.Atan2(player.transform.position.y - endRoom.transform.position.y, player.transform.position.x - endRoom.transform.position.x) * Mathf.Rad2Deg;
        transform.Rotate(0f, direction, 0f);
    }

    #endregion
}
