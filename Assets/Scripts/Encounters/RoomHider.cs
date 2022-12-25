/* 
 * Programmers: Jack Kennedy
 * Purpose: set our camera background color according to the floor #
 * Inputs: 2 colors, one for inside and one for outside
 * Outputs: the camera background color
 */

using UnityEngine;

public class RoomHider : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite outsideSprite;
    [SerializeField] private Sprite insideSprite;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        if (FloorManager.floor == 1)
        {
            foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sprite = outsideSprite;
            }
        }
        else
        {
            foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sprite = insideSprite;
            }
        }
    }

    #endregion
}
