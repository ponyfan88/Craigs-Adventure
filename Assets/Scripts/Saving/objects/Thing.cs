/* 
 * Programmers: Jack Kennedy
 * Purpose: global math functions
 * Inputs: various inputs, usually being called with a number
 * Outputs: mirrors inputs
 */

using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(healthManager))]
public class Thing : MonoBehaviour
{
    public thingEnums.thingType thingType;

    public thingEnums.thingPrefab thingPrefab;
}