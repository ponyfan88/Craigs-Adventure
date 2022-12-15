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

    private Sprite currentTowerSprite;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        currentTowerSprite = GetComponent<SpriteRenderer>().sprite;

        // we dont need to do this every frame since every new floor we restart the scene
        if (FloorManager.floor == 1)
        {
            currentTowerSprite = outsideTower;
        }
        else
        {
            currentTowerSprite = tower;
        }
    }

    #endregion
}
