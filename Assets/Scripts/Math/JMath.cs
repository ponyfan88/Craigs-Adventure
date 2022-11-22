/* 
 * Programmers: Jack Kennedy
 * Purpose: global math functions
 * Inputs: none
 * Outputs: math functions
 */

using UnityEngine;
using System.IO;

public static class JMath
{
    /*
     * purpose: mod function, supporting negatives
     * inputs: a and b, getting a within b (it wraps around, so if a is 361 and b is 360 we return 1)
     * outputs: a mod b (negatives supported)
     */
    public static float mod(float a, float b = 360)
    {
        float c = a % b;
        // if either c or b is negative (xor)
        if ((c < 0 && b > 0) || (c > 0 && b < 0))
        {
            c += b; // add b to c
        }
        return c;
    }
}
