/* 
 * Programmers: Xander Mooney
 * Purpose: Manage the health of EVERY object that needs health
 * Inputs: health values, functions that take or give damage
 * Outputs: whether the object has died; the actual health
 */

using UnityEngine;

public class healthManager : MonoBehaviour
{
    #region Variables

    Pause Pause; // import pausing
    public int maxHealth = 5, health = 5;
    public float invulnerabilityTime;
    float invulnerabilityTimer;
    public bool takePlayerDamage = true;
    SoundManager soundManager;
    EffectsManager effectsManager;
    SavesManager savesManager;

    #endregion

    #region Default Methods

    public void Awake()
    {
        health = maxHealth;

        Pause = GameObject.Find("UI").GetComponent<Pause>(); // find our pause script
        soundManager = FindObjectOfType<SoundManager>(); // gets the soundManager
        effectsManager = FindObjectOfType<EffectsManager>(); // gets the effectsManager
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

    // inputs damage taken, and whether or not to ignore invulnerabilityTimer
    // also allows you to output a bool when calling this function, which will tell you if the object took damage, or ignored it due to invulnerability.
    public bool TakeDamage(int damage, bool ignoreInvulnerable = false)
    {
        if (invulnerabilityTimer < Time.time || ignoreInvulnerable) // take damage if Timer is over OR if ignoring it
        {
            health -= damage; // subtract damage
            
            if (damage > 0 && !ignoreInvulnerable) 
                invulnerabilityTimer = Time.time + invulnerabilityTime; // set invulnerabilityTimer

            if (gameObject.name == "player") // if its the player
            {
                LogToFile.Log("Player took damage " + damage.ToString());

                //flash
                effectsManager.addEffect(gameObject, GlobalFX.effect.flashTransparent, 1, new Color(1f, 1f, 1f, 1f), 1, 3);
                //GlobalFX.effect.flashTransparent

                soundManager.Play("Hurt"); // play the hurt sound
            }
            else if (!gameObject.TryGetComponent(out Pickupable pickup)) // not the player or a throwable item
            {
                //flash
                effectsManager.addEffect(gameObject, GlobalFX.effect.flash, 1, new Color(1f, 0f, 0f, 1f), 1, 3);

                soundManager.Play("Hurt"); // play the hurt sound
            }

            if (health <= 0) // if object has ran out of health
            {
                if (gameObject.name == "player") // if its the player
                {
                    LogToFile.Log("player has died");
                    Pause.EndGame(); // display the death screen
                }
                // if the object is a projectile, we only disable the object so that the projectile scripts can reuse it without crashing
                else if (TryGetComponent(out Projectile proj))
                {
                    gameObject.SetActive(false);
                }
                    else
                    {
                        // anything here is an enemy / not the player
                        // For now we just kill the object.
                        Destroy(this.gameObject);

                        LogToFile.Log("killed " + this.gameObject.ToString());
                    }
            }
            return true; // tell the function the object took damage
        }
        else return false; // tell the function the object did not take damage
    }
    
    public void Heal(int healAmount) // inputs amount to heal
    {
        health += healAmount;
        LogToFile.Log("healed player by " + healAmount.ToString());
        if (health > maxHealth) // to prevent an "overheal" situation, if our health is over the maxhealth we set it back to maxhealth.
        {
            health = maxHealth;
            LogToFile.Log("player was overhealed, set health to " + health.ToString());
        }
    }

    public void SetHealth(int setAmount) 
    {
        health = setAmount;

        LogToFile.Log("set health to " + health.ToString());

        if (health <= 0)
        {
            TakeDamage(0, true); // taking damage of 0 just to trigger the proper death handling mechanics
        }
        else if (health >= maxHealth) // avoids overhealing
        {
            health = maxHealth;
        }
    }

    #endregion
}
