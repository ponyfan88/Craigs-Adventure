/* 
 * Programmers: Jack Kennedy
 * Purpose: displays correct BG depending if we are outside or not
 * Inputs: floor
 * Outputs: the correct floor bg
 */


using UnityEngine;

public class TowerBase : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite outsideTower; // tower when floor == 1
    [SerializeField] private Sprite tower; // tower when floor >= 2

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        // we dont need to do this every frame since every new floor we restart the scene
        if (FloorManager.floor == 1)
        {
            GetComponent<SpriteRenderer>().sprite = outsideTower;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = tower;
        }
    }

    #endregion
}
