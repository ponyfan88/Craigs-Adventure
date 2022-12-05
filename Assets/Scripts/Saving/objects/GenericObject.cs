/* 
 * Programmers: Jack Kennedy
 * Purpose: Saves GameObject and (abitrarily) another type
 * Inputs: a GameObject, any object
 * Outputs: (mirrors inputs)
 */

using System.Collections.Generic;
using UnityEngine;

public class GenericObject
{
    // both equal null by default
    public GameObject thingParent = null;
    public GameObject itemEnemyThing = null;
    public int parentUniqueID = 0;
}
