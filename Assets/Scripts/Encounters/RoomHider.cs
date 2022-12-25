/* 
 * Programmers: Jack Kennedy
 * Purpose: set our room hider to the correct color at startup
 * Inputs: 2 sprites - inside and outside walls
 * Outputs: the room hider children sprites
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
