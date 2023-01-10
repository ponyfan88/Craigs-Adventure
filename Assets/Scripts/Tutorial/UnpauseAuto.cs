/* 
 * Programmers: Christopher Kowalewski 
 * Purpose: Unpauses the tutorial automatically on start, since due to loading room gen in the game scene, it starts paused
 * Inputs: Only loads in tutorial
 * Outputs: Unpauses the game and sets tutorial to active
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseAuto : MonoBehaviour
{
    Pause pausing;

    // Start is called before the first frame update
    void Start()
    {
        pausing = gameObject.GetComponent<Pause>();

        pausing.EndLoading();

        TutorialManager.TutorialActive = true;
    }
}
