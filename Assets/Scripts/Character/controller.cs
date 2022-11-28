/* 
 * Programmers: Xander Mooney & Jack Kennedy
 * Purpose: Detect and use inputs from the controller to control the main character and his velocity for later use
 * Inputs: controller / keyboard inputs
 * Outputs: Velocity and moves object
 */

using System;
using UnityEngine;

public class controller : MonoBehaviour
{
    #region Variables

    public float dashMoveCooldown, speed = 6f;
    [NonSerialized] public float xMov, yMov;
    [SerializeField] float dashDuration, dashVelocity, dashCooldown;
    float dashCooldownTimer, dashDurationTimer, knockbackTime;
    [NonSerialized]public Vector2 finalVelocity, moveDirection = new Vector2(1, 0);
    Vector2 knockbackVelocity;
    [NonSerialized] public bool canMove = true, isDashing = false;
    Rigidbody2D rb;
    SoundManager soundManager;
    TrailRenderer dashParticle;
    SavesManager savesManager;

    #endregion

    #region Default Methods

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // gets rigidbody
        dashParticle = GameObject.Find("Dash").GetComponent<TrailRenderer>(); // gets the trail renderer for dashing
        soundManager = FindObjectOfType<SoundManager>(); // gets the soundManager

        knockbackTime = 0; // setting to a default value to avoid nulls and errors
        
        savesManager = FindObjectOfType<SavesManager>();

        if (savesManager.loadingSave)
        {
            rb.position = new Vector2(savesManager.currentSave.playerx, savesManager.currentSave.playery);
        }
    }
    
    private void Update()
    {
        // we test for input dashing here, as doing it in FixedUpdate leads to dropping inputs
        if (Input.GetButtonDown("Dash") && !isDashing && !Pause.paused)
        {
            isDashing = true;
        }
    }
    
    void FixedUpdate()
    {
        // if we cant move we skip most of the script
        if (canMove && dashMoveCooldown < Time.time && !Pause.paused)
        {
            finalVelocity = new Vector2(0, 0); // resets velocities
            // calculates inputs via unity input manager
            xMov = Input.GetAxis("Horizontal");
            yMov = Input.GetAxis("Vertical");

            /* if:
             * 1. we are pressing our dash button
             * 2. we arent already dashing
             * 3. our dash cooldown has expired
             * 4. we arent paused
             */
            if (isDashing && dashCooldownTimer < Time.time && !Pause.paused)
            {
                // set our duration & cooldown timers
                dashDurationTimer = Time.time + dashDuration;
                dashCooldownTimer = dashDurationTimer + dashCooldown;
                // cooldown for our movement a short time after dashing
                dashMoveCooldown = dashDurationTimer + 0.1f;
                soundManager.Play("Dash"); // play dash sound
                dashParticle.enabled = true; // enable the dash trail
            }

            if (yMov != 0 || xMov != 0) // if we moved (wasd)
            {
                moveDirection = new Vector2(Math.Sign(xMov), Math.Sign(yMov)); // calculate movement direction
                // calculate the velocity of our movement
                finalVelocity = new Vector2(transform.position.x + xMov * speed * Time.fixedDeltaTime, transform.position.y + yMov * speed * Time.fixedDeltaTime);
                rb.MovePosition(finalVelocity); // apply the movement velocity
            }
        }
        else if (Time.time < dashDurationTimer)
        {
            // take our current position (new Vector2(transform.position.x, transform.position.y, add our dash (moveDirection * dashVelocity), * make it time independant (Time.fixedDeltaTime)
            rb.MovePosition((Vector2)transform.position + moveDirection * dashVelocity * Time.fixedDeltaTime);
        }
        else if (Time.time > dashDurationTimer)
        {
            dashParticle.enabled = false; // disable our dash trail
            isDashing = false; // we arent dashing anymore, set to false
        }

        if (Time.time < knockbackTime) // check if you are currently within the time-frame to get pushed back by
        {
            //applies movement calculated in ApplyKnockback
            rb.MovePosition(new Vector2(transform.position.x + knockbackVelocity.x * Time.fixedDeltaTime, transform.position.y + knockbackVelocity.y * Time.fixedDeltaTime));
        }
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void ApplyKnockback(Vector2 collisionPos) // this method is used for knocking back the player based on the objects position; mainly for colliding with enemies
    {
        // uses the distance from the player to the object colliding to find the vector to push the player back from
        knockbackVelocity = new Vector2(Mathf.Round(transform.position.x - collisionPos.x) * 20, Mathf.Round(transform.position.y - collisionPos.y) * 20);
        knockbackTime = Time.time + 0.15f; // sets the knockbackTime to last for .15 of a second
    }
    
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void ApplyKnockback(float bulletRotation) // this method is for knocking back the player based on the objects rotation; used for bullets
    {
        // turns rotation of the object into a radian       
        float radian = Mathf.Deg2Rad * bulletRotation;
        // uses cosine and sine to turn the radian of the bullet into a direction to push the player
        knockbackVelocity = new Vector2(Mathf.Cos(radian) * 20, Mathf.Sin(radian) * 20);
        // sets the knockbackTime to last for .15 of a second
        knockbackTime = Time.time + 0.15f;
    }

    #endregion
}
