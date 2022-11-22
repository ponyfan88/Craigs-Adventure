/* 
 * Programmers: Jack Kennedy
 * Purpose: Manages effects
 * Inputs: when other scripts call for an effect
 * Outputs: on screen effects
 */

using UnityEngine;

public class FloorManager : MonoBehaviour
{

    #region Variables

    // make static because scripts shouldnt have to grab the floor manager
    public static byte floor = 1; // floor

    public static bool loadSaveOverride = false;

    #endregion

    #region Default Methods

    public void Start()
    {
        loadSaveOverride = false;

        // get every saving object
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Floor Management");

        // if we have more than one of them
        if (objs.Length > 1)
        {
            // DESTROY IT
            Destroy(this.gameObject);
        }

        // make sure we dont destroy it or anything like that LOLOLOLOL
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion
}