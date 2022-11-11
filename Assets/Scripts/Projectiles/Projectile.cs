/* 
 * Programmers: Anmol Acharya and Jack Kennedy
 * Purpose: store projectile data and movement
 * Inputs: projectile spawner script
 * Outputs: projectile with data
 */

using UnityEngine;
using BulletEffects;

public class Projectile : MonoBehaviour
{
    #region Variables

    public Vector3 velocity; // our parents velocity
    public float speed;// how fast the bullets 
    public float rotation; // bullets rotations
    public float lifetime; // how long the bullet will be active
    public Bulletpaths bulletPath; // how the bullet will move
    public collisonEffect onCollison;// decides what happens when a projectile collides with wall
    public endlifeEffect endlifeEffect; // when the projectiles lifespan ends it will perform this action
    public float SpecialEffect; // the ammount that we move it back and forth
    float timer; // countdown till death

    #endregion

    #region Default Methods

    void Start()
    {
        timer = lifetime; // set up a timer
    }
    void Update()
    {
        if (!Pause.paused)
        {
            switch (bulletPath) //tell our projectile how to move
            {
                case Bulletpaths.linear:
                    /* jack kennedy presents
                     * THE ROTATE BULLET TRANSLATION EQUATION!!!!
                     * first, we'll grab our rotation with the variable `rotation`, converting from degrees to radians using `Mathf.Deg2Rad`
                     * next, we get a `Vector3` (we dont set z) that represents our rotation with a radius of 1.
                     * next, multiply that by `Time.deltaTime` (the time since last frame) times the `speed` of our projectile
                     * now that we have our normal projectile, we add our enemies velocity, just so that bullets look nicer and function more realistically.
                     * the enemies velocity is represented with `velocity` and we multiply it by `Time.deltaTime`
                     */
                    transform.Translate(new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)) * Time.deltaTime * speed + velocity * Time.deltaTime);
                    break;
                case Bulletpaths.sineWave:
                    /* jack kennedy presents
                     * THE CURVY BULLET TRANSLATION EQUATION!!!!
                     * pointToRotate represents our bullet, represented as a point moving left to right, back and forth.
                     * we use `modulate` to set this, since it represents the number we multiply realtimesincestartup by. (side note here: technically, our program will *eventually* stop curving bullets and will just return broken values.)
                     * we set our y to 1 since thats easy to multiply
                     * next, we rotate that point in 2d space by creating a new `Vector3`, grabbing our rotation, subtracting 90 from it (so that it is oriented correctly) and multiplying it by `Mathf.Deg2Rad`.
                     * im not going to explain the math here since this goes into trigenometry/calculus, but it basically rotates the point
                     * next we multiply by the time since last frame and the speed.
                     * then we add our enemies current velocity (as a vector)
                     * and BLAM we got a bullet in space that wobbles.
                     */
                    Vector3 pointToRotate = new Vector3(Mathf.Cos(SpecialEffect * Time.realtimeSinceStartup), 1);
                    transform.Translate(new Vector3(pointToRotate.x * Mathf.Cos((rotation - 90) * Mathf.Deg2Rad) - pointToRotate.y * Mathf.Sin((rotation - 90) * Mathf.Deg2Rad), pointToRotate.y * Mathf.Cos((rotation - 90) * Mathf.Deg2Rad) + pointToRotate.x * Mathf.Sin((rotation - 90) * Mathf.Deg2Rad)) * Time.deltaTime * speed + velocity * Time.deltaTime);
                    break;
                case Bulletpaths.SpeedChange:
                    transform.Translate(new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)) * Time.deltaTime * speed + velocity * Time.deltaTime);
                    break;
                default: // incase of no set path, set to linear
                    transform.Translate(new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)) * Time.deltaTime * speed + velocity * Time.deltaTime);
                    break;
            }
        }
        timer -= Time.deltaTime;
        // once our timer expires
        if (timer <= 0)
        {
            switch (endlifeEffect)
            {
                case endlifeEffect.none:
                    gameObject.SetActive(false); // deactivate the projectile
                    break;
                default:
                    gameObject.SetActive(false); // deactivate the projectile
                    break;
            }
        }
    }

    #endregion

    #region Custom Methods

    public void ResetTimer()
    {
        timer = lifetime; // reset our bullet so that it once again lives and breathes
    }

    #endregion
}
