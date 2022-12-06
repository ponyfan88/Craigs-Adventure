/* 
 * Programmers: Jack Kennedy
 * Purpose: Saves GameObject and (abitrarily) another type
 * Inputs: a GameObject, any object
 * Outputs: (mirrors inputs)
 */

using UnityEngine;

public class GenericObject
{
    // both equal null by default
    public thingEnums.thingPrefab thingPrefab;
    public int uniqueID = 0;
    public Vector3 position;
    public int health;
}
