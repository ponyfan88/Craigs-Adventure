/* Programmer: Xander Mooney
 * Purpose: Find a point between the dragon and player that is still close to the player
 * Inputs: positions of dragon and player
 * Outputs: position of the camera
 */
using UnityEngine;

public class CameraFollowControl : MonoBehaviour
{
    Transform player;
    Transform dragon;
    private void Awake()
    {
        player = GameObject.Find("player").transform;
        dragon = GameObject.Find("Dragon").transform;
    }
    void Update()
    {
        gameObject.transform.position = new Vector2(Mathf.Lerp(player.position.x, dragon.position.x, 0.25f),
                                                    Mathf.Lerp(player.position.y, dragon.position.y, 0.25f));
    }
}
