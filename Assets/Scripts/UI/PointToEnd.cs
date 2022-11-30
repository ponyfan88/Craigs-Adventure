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

    public float offset = 0f;

    private float direction = 0f;

    private GameObject player;
    private GameObject endRoom;

    private bool gotdist = false;
    private float startingDistance;

    SpriteRenderer pointer;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player"); // grab our player
        endRoom = GameObject.FindWithTag("End Room");
        pointer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (endRoom != null) // if we've found the endroom
        {
            // only get the starting distance on the first frame we find the end room
            if (!gotdist)
            {
                startingDistance = JMath.Distance(endRoom.transform, player.transform);
                gotdist = true;
            }

            direction = Mathf.Atan2(endRoom.transform.position.y - player.transform.position.y, endRoom.transform.position.x - player.transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, direction);

            // scale pointer alpha with our distance, the closer we are the less we should see the pointer
            pointer.color = new Color(1f, 1f, 1f, JMath.Distance(endRoom.transform, player.transform) / startingDistance);
        }
        else // we cannot find the endroom
        {
            // try and find it for next frame
            endRoom = GameObject.FindWithTag("End Room");
        }
    }

    #endregion
}
