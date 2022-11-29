/* 
 * Programmers: Jack Kennedy
 * Purpose: global math functions
 * Inputs: various inputs, usually being called with a number
 * Outputs: math functions
 */

using System;
using UnityEngine; // unused

public static class JMath
{
    /*
     * purpose: mod function, supporting negatives
     * inputs: a and b, getting a within b (it wraps around, so if a is 361 and b is 360 we return 1)
     * outputs: a mod b (negatives supported)
     */
    public static float Mod(float a, float b = 360)
    {
        // we get the normal mod (as a float)
        float c = a % b;
        // if one number is negative and the other is not
        if ((c < 0 && b > 0) || (c > 0 && b < 0))
        {
            // we add our divisor to our first number.
            c += b;
        }

        // float should be good now.
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

    // see above; overload for doubles
    public static bool HasDecimals(this double a)
    {
        return a % 1 == 0;
    }

    // see above; overload for decimals
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
        return (int)(a * Math.Pow(10, a - (int)a));
    }

    /*
     * purpose: divides a long by 10 until it can be an int
     * inputs: a long to operate on ("a")
     * outputs: top half of a long to be an int
     */
    public static int Top(this long a)
    {
        // store a as b to divide it later
        long b = a;
        while (Math.Abs(b) > int.MaxValue) // while b does not exceed the positive or negative 32 bit integer limit
        {
            b /= 10; // divide it by 10
        }
        return (int)b; // return it as an int
    }

    /*
     * purpose: gets the bottom half of a long so that it is a valid integer
     * inputs: a long to operate on ("a")
     * outputs: top half of a long to be an int
     */
    public static int Bottom(this long a)
    {
        // convert a to a string
        string b = a.ToString();
        // this is our 3rd variable, c. we'll use this for our final number
        long c = 0;
        
        // for every digit in our long
        for (int i = 0; i < b.Length; ++i)
        {
            // parse it as an integer
            // we use b.Length - 1 - i as to get the FIRST digit at 0 rather than getting the LAST digit at 0
            long d = byte.Parse(b[b.Length - 1 - i].ToString()) * (long)Math.Pow(10, i);
            /*
             * breaking this down:
             * "byte.Parse(b[b.Length - 1 - i].ToString())" gives us a byte representing the digit
             * "* (long)Math.Pow(10, i)" multiplies our number by a power of ten.
             * we do this because the first digit is in the ones place, the second the tens, etc.
             */

            // if the value of our total parsed integers added together exceeds the max value an int should be
            if (Math.Abs(c + d) > int.MaxValue)
            {
                // then we'll return our int
                return (int)c;
            }
            else
            {
                // otherwise, add our parsed number to our counter.
                c += d;
            }
        }

        // if all that checks out, our number was within an int. we can then return it.
        return (int)c;
    }

    /*
     * purpose: returns the length of a number
     * inputs: an int to operate on ("a")
     * outputs: the number of numbers in a number
     */
    public static int Length(this int a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload for longs
    public static int Length(this long a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload for floats
    public static int Length(this float a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload for doubles
    public static int Length(this double a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload for bytes
    public static int Length(this byte a)
    {
        return (int)Math.Floor(Math.Log10(Math.Abs(a)) + 1);
    }

    // see above; overload for decimals
    public static int Length(this decimal a)
    {
        // convert to a non-negative, then to a string, get rid of decimal marks, and get the length
        // this is because Math.Log10 does not take decimals, nor does unity's Mathf.Log10
        return Math.Abs(a).ToString().Replace(".", string.Empty).Length;
    }
    
    /*
     * purpose: returns the distance from two vector2's
     * inputs: two specified vector2's
     * outputs: a float to specify the distance
     */
    public static float Distance(Vector2 a, Vector2 b)
    {
        // a^2 + b^2 = c^2, so we find the hypotenuse by finding the x and y distances and putting them to the power of 2, and then the square root for c, which is the distance
        return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
    }
    
    // see above; overload for transforms
    public static float Distance(Transform a, Transform b)
    {
        // a^2 + b^2 = c^2, so we find the hypotenuse by finding the x and y distances and putting them to the power of 2, and then the square root for c, which is the distance
        return (float)Math.Sqrt(Math.Pow(a.position.x - b.position.x, 2) + Math.Pow(a.position.y - b.position.y, 2));
    }
}
