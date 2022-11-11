/*
 * Programmers: Xander Mooney
 * Purpose: Allow the character to attack
 * Inputs: enemy health, collision box for attacking
 * Outputs: damage
 */

using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    #region Variables

    BoxCollider2D hitBox;
    controller player;
    itemManager itemMan;
    Animator animator;
    SoundManager soundManager;
    playerWalkanim animationControl;
    AIManager EnemyAImanager;
    Vector2 attackDir = new Vector2(0, 0);
    public Vector2 hitboxSize = new Vector2(3,2);
    public float attackDuration = 1f, attackCooldown = 2f;
    float attackCooldownTimer;
    public int attackDamage = 2;
    public bool isAttacking = false;
    
    #endregion
    
    #region Default Methods

    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>(); // hitbox for dealing damage
        player = GetComponent<controller>(); // player controlling script
        itemMan = GetComponent<itemManager>(); // item controlling script
        animator = GetComponent<Animator>(); // animator for the player
        animationControl = animator.GetBehaviour<playerWalkanim>(); // Gets the behaviour that controls the player walking anim
        soundManager = FindObjectOfType<SoundManager>(); // gets the soundManager
    }
    void Update()
    {
        if (Input.GetButtonDown("Attack") && attackCooldownTimer < Time.time && !player.isDashing) // if attacking, not on attack cooldown, and not dashing
        {
            player.speed /= 2;

            if (player.moveDirection.x != 0) // if moving X, we want to prioritize it over the Y direction
            {
                hitBox.size = hitboxSize; // make the hitbox it appropriate size
                if (animationControl.facingRight)
                {
                    hitBox.offset = new Vector2((hitboxSize.x / 2) + .6f, 0); // sets the hitbox to its correct pos
                    attackDir = new Vector2(1, 0);
                }
                else
                {
                    hitBox.offset = new Vector2(-(hitboxSize.x / 2) - .6f, 0); // if our player is flipped, we will flip our attack hitbox
                    attackDir = new Vector2(-1, 0);
                }
            }
            else // we are hitting in the Y direction
            {
                hitBox.size = new Vector2(hitboxSize.y, hitboxSize.x); // make the hitbox its appropriate size
                if (player.moveDirection.y > 0)
                {
                    hitBox.offset = new Vector2(0, 1.1f + (hitboxSize.x / 2)); // sets the hitbox to its correct pos
                    attackDir = new Vector2(0, 1);
                }
                else
                {
                    hitBox.offset = new Vector2(0, -1.1f - (hitboxSize.x / 2)); // sets the hitbox to its correct pos
                    attackDir = new Vector2(0, -1);
                }
            }
            hitBox.enabled = true; // enable hitbox
            attackCooldownTimer = Time.time + attackCooldown; // sets attack cooldown
            animator.SetBool("isAttacking", true); // plays attack animation
            Invoke("DisableAttack", attackDuration); // disables attack after the attack cooldown
            isAttacking = true; // we are now attacking
            soundManager.Play("Miss");

            if (itemMan.holdingItem)
            {
                itemMan.selectedItem.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitBox.IsTouching(collision)) // makes sure the collision occurs from the attack hitbox, and not the players normal collision hitbox
        {
            // tries to get the collisions health component
            bool hasHealth = collision.gameObject.TryGetComponent<healthManager>(out healthManager enemyHealth);

            if (hasHealth) if (enemyHealth.takePlayerDamage) // if object has health and is marked to be damaged by the player
            {
                bool tookDamage = enemyHealth.TakeDamage(attackDamage); // deals damage that doesn't ignore invulnerability
                bool isEnemy = collision.gameObject.TryGetComponent<AIManager>(out EnemyAImanager);
                
                if (tookDamage && isEnemy) 
                {
                        // if these conditions are true, we want to knockback the enemy
                        EnemyAImanager.ApplyKnockback(attackDir);
                }
            }
        }
    }

    #endregion

    #region Custom Methods

    void DisableAttack() // used to disable the attack hitbox after specified time
    {
        player.speed *= 2;
        hitBox.enabled = false; // disable hitbox
        animator.SetBool("isAttacking", false); // turn off animation
        isAttacking = false; // we are no longer attacking
        if (itemMan.holdingItem)
        {
            itemMan.selectedItem.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    #endregion
}
