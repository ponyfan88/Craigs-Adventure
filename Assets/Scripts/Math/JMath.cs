/* 
 * Programmers: Jack Kennedy
 * Purpose: global math functions
 * Inputs: various inputs, usually being called with a number
 * Outputs: math functions
 */

public static class JMath
{
    #region Variables

    public const int MAXINT = 2147483647;
    public const long MAXLONG = 9223372036854775807;

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
    public static bool HasDecimals(float a)
    {
        return a % 1 == 0;
    }

    // see above; overload
    public static bool HasDecimals(double a)
    {
        return a % 1 == 0;
    }

    // see above; overload
    public static bool HasDecimals(decimal a)
    {
        return a % 1 == 0;
    }

    /*
     * purpose: multiplies a number by 10 until it no longer has decimals
     * inputs: a float to test with ("a")
     * outputs: if the number has a decimal or not
     */
    public static int LargerTillInt(float a)
    {
        float b = a;
        while (!HasDecimals(b)) // while b does not have a decimal
        {
            b *= 10; // multiply it by 10
        }
        return (int)b; // return it as an int
    }

    // see above; overload
    public static int LargerTillInt(double a)
    {
        double b = a;
        while (!HasDecimals(b)) // while b does not have a decimal
        {
            b *= 10; // multiply it by 10
        }
        return (int)b; // return it as an int
    }

    // see above; overload
    public static int LargerTillInt(decimal a)
    {
        decimal b = a;
        while (!HasDecimals(b)) // while b does not have a decimal
        {
            b *= 10; // multiply it by 10
        }
        return (int)b; // return it as an int
    }

    /*
     * purpose: devides a long by 10 until it can be an int
     * inputs: a long to operate on ("a")
     * outputs: top half of a long to be an int
     */
    public static int IntFromTopOfLong(long a)
    {
        long b = a;
        while (b >= JMath.MAXINT) // while b does not have a decimal
        {
            b /= 10; // multiply it by 10
        }
        return (int)b; // return it as an int
    }

    #endregion
}
