/* 
 * Programmers: Jack Kennedy
 * Purpose: set our camera background color according to the floor #
 * Inputs: 2 colors, one for inside and one for outside
 * Outputs: the camera background color
 */

using UnityEngine;

public class CameraBackgroundColor : MonoBehaviour
{
    #region Variables

    [SerializeField] private Color outsideColor; // #1c7037 
    [SerializeField] private Color insideColor; // #222034

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        if (FloorManager.floor == 1)
        {
            Camera.main.backgroundColor = outsideColor;
        }
        else
        {
            Camera.main.backgroundColor = insideColor;
        }
    }

    #endregion
}
