/*
 * Programmers: Christopher Kowalewski
 * Purpose: Stores info on dialogue from characters in cutscenes
 * Inputs: Serialized input from unity editor
 * Outputs: Public variables for the name of the speaker and the lines of dialog they are saying
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;

    [TextArea(1, 5)]
    public string[] lines;
}
