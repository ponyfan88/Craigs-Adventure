/* 
 * Programmers: Xander Mooney
 * Purpose: Manage the health of EVERY object that needs health
 * Inputs: health values, functions that take or give damage
 * Outputs: whether the object has died; the actual health
 */
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class healthManager : MonoBehaviour
{
    #region Variables

    Pause Pause; // import pausing
    public int maxHealth = 5, health = 5;
    public float invulnerabilityTime;
    float invulnerabilityTimer;
    public bool takePlayerDamage = true;
    SoundManager soundManager;
    SavesManager savesManager;

    // enum to decide what should happen if an object were to die.
    public enum DestroyEvent { destroy, summonProjectile };
    public DestroyEvent destroyEvent = DestroyEvent.destroy;
    [SerializeField] int destroyProjectileIndex;
    // enum to decide what should happen if an object were damaged.
    public enum DamagedEvent { nothing, displayParticle };
    public DamagedEvent damagedEvent = DamagedEvent.nothing;
    
    #endregion

    #region Default Methods

    public void Awake()
    {
        health = maxHealth;

        Pause = GameObject.Find("UI").GetComponent<Pause>(); // find our pause script
        soundManager = FindObjectOfType<SoundManager>(); // gets the soundManager
        invulnerabilityTimer = Time.time; // Set invulnerabilityTimer to avoid null output
        savesManager = FindObjectOfType<SavesManager>();

        if (savesManager.loadingSave)
        {
            health = savesManager.currentSave.playerHealth;
            maxHealth = savesManager.currentSave.playerMaxHealth;
        }
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: Allow for other scripts to damage an object by a set amount
     * inputs: damage taken, and whether or not to ignore invulnerabilityTimer
     * outputs: a bool to say if the enemy was damaged, and the damage itself
     */
    public bool TakeDamage(int damage, bool ignoreInvulnerable = false)
    {
        if (invulnerabilityTimer < Time.time || ignoreInvulnerable) // take damage if Timer is over OR if ignoring it
        {
            health -= damage; // subtract damage
            
            if (damage > 0 && !ignoreInvulnerable) 
                invulnerabilityTimer = Time.time + invulnerabilityTime; // set invulnerabilityTimer

            if (gameObject.name == "player") // if its the player
            {
                LogToFile.Log("Player took damage " + damage);

                //flash
                EffectsManager.AddEffect(gameObject, GlobalFX.effect.flashTransparent, 1, Color.white, 1, 3); 
                //GlobalFX.effect.flashTransparent

                soundManager.Play("Hurt"); // play the hurt sound
            }
            else if (!gameObject.TryGetComponent(out Item item)) // not the player or a throwable item
            {
                //flash
                EffectsManager.AddEffect(gameObject, GlobalFX.effect.flash, 1, Color.red, 1, 3);

                soundManager.Play("Hurt"); // play the hurt sound
            }

            if (health <= 0) // if object has ran out of health
            {
                // trigger death handling mechanics
                Death();
                LogToFile.Log("killed " + gameObject);
               
            }
            return true; // tell the function the object took damage
        }
        else return false; // tell the function the object did not take damage
    }
    
    /*
     * purpose: Allow for other scripts to heal an object by a set amount
     * inputs: value
     * outputs: health with added value
     */
    public void Heal(int healAmount) // inputs amount to heal
    {
        health += healAmount;
        LogToFile.Log("healed " + gameObject + " by " + healAmount.ToString());
        if (health > maxHealth) // to prevent an "overheal" situation, if our health is over the maxhealth we set it back to maxhealth.
        {
            health = maxHealth;
            LogToFile.Log(gameObject + " was overhealed, set health to " + health.ToString());
        }
    }

    /*
     * purpose: Allow for other scripts to set the health of an object to a set value
     * inputs: value
     * outputs: health as value
     */
    public void SetHealth(int setAmount) 
    {
        health = setAmount;

        LogToFile.Log(gameObject.name + " set health to " + health.ToString());

        if (health <= 0)
        {
            TakeDamage(0, true); // taking damage of 0 just to trigger the proper death handling mechanics
        }
        else if (health >= maxHealth) // avoids overhealing
        {
            health = maxHealth;
        }
    }
    /*
    * purpose Kill GameObjects
    * inputs: destroy event
    * outputs: none
    */
    public void Death()
    {
        switch (destroyEvent)
        {
            case DestroyEvent.summonProjectile:
                {
                    if (TryGetComponent(out ProjectileSpawner spawner))
                    {
                        spawner.spawnerController(destroyProjectileIndex);
                        GetComponent<AIManager>().canMove = false;
                        Invoke("InvokeDestoryObject", 2.1f);//sorry this was the only way i could think of doing this -Anmol Acharya
                    }
                    else Destroy(gameObject);
                    break;
                }
            default:
                if (gameObject.name == "player") // if its the player
                {
                    LogToFile.Log("player has died");
                    Pause.EndGame(); // display the death screen
                }
                else if (TryGetComponent(out Projectile proj))
                {
                    gameObject.SetActive(false);
                }
                else
                {
                Destroy(gameObject);
                }
                return;
        }
    }
    /*
 * purpose: use invoke to destroy game objects
 * inputs: none
 * outputs: none
 */
    void InvokeDestoryObject()
    {
        Destroy(gameObject);
    }
    #endregion
}
