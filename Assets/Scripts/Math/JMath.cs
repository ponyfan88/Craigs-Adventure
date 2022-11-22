/* 
 * Programmers: Jack Kennedy
 * Purpose: global math functions
 * Inputs: various inputs, usually being called with a number
 * Outputs: math functions
 */

using System;
using UnityEngine;

public static class JMath
{
    #region Variables

    public const int MAXINT = 2147483647;
    public const long MAXLONG = 9223372036854775807;

    public const byte MAXINTLENGTH = 10;
    public const byte MAXLONGLENGTH = 19;

    #endregion

    #region Custom Methods

    /*
     * purpose: mod function, supporting negatives
     * inputs: a and b, getting a within b (it wraps around, so if a is 361 and b is 360 we return 1)
     * outputs: a mod b (negatives supported)
     */
    public static float Mod(float a, float b = 360)
    {
        float c = a % b;
        // if one number is negative and the other is not
        if ((c < 0 && b > 0) || (c > 0 && b < 0))
        {
            c += b; // add b to c
        }
        return c;
    }

    /*
     * purpose: tells you if a number has a decimal
     * inputs: a float to test with ("a")
     * outputs: if the number has a decimal or not
     */
    public static bool HasDecimals(this float a)
    {
        return a % 1 == 0;
    }

    // see above; overload
    public static bool HasDecimals(this double a)
    {
        return a % 1 == 0;
    }

    // see above; overload
    public static bool HasDecimals(this decimal a)
    {
        return a % 1 == 0;
    }

    /*
     * purpose: multiplies a number by 10 until it no longer has decimals
     * inputs: a float to test with ("a")
     * outputs: if the number has a decimal or not
     */
    public static int LargerTillInt(this float a)
    {
        float b = a;
        byte c = 0;
        while (!HasDecimals(b) && c <= JMath.MAXINTLENGTH) // while b does not have a decimal
        {
            b *= 10; // multiply it by 10
            ++c;
        }
        return (int)b; // return it as an int
    }

    /*
     * purpose: devides a long by 10 until it can be an int
     * inputs: a long to operate on ("a")
     * outputs: top half of a long to be an int
     */
    public static int IntFromTopOfLong(this long a)
    {
        long b = a;
        byte c = 0;
        while (b >= JMath.MAXINT && c <= JMath.MAXINTLENGTH) // while b does not have a decimal
        {
            b /= 10; // multiply it by 10
            ++c;
        }
        return (int)b; // return it as an int
    }

    /*
     * purpose: returns the lenght of a number
     * inputs: an int to operate on ("a")
     * outputs: the number of numbers in a number
     */
    public static int Length(this int a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload
    public static int Length(this long a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload
    public static int Length(this float a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload
    public static int Length(this double a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload
    public static int Length(this byte a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload
    public static int Length(this decimal a)
    {
        // convert to a non-negative, then to a string, get rid of decimal marks, and get the length
        // this is because Math.Log10 does not take decimals, nor does unity's Mathf.Log10
        return Math.Abs(a).ToString().Replace(".", string.Empty).Length;
    }


    #endregion
}
