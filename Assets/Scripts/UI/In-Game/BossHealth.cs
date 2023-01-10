/* 
 * Programmers: Jack Kennedy
 * Purpose: display boss health
 * Inputs: boss health and if we are paused
 * Outputs: a large health bar on screen
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    #region Variables

    // health we store to update
    [NonSerialized] private int storedHealth = -1;

    // the boss
    [SerializeField] private GameObject boss;

    // the bars themselves
    [SerializeField] private GameObject bossBarFG;
    [SerializeField] private GameObject bossBarBG;

    // our boss health
    [NonSerialized] private healthManager bossHealth;

    // bool for if our boss has died
    [NonSerialized] private bool bossDied = false;

    // bool for if we've begun fading to black
    [NonSerialized] private bool startedFade = false;

    // var representing time that has passed, used in various things
    [NonSerialized] private float timer = 0f;

    // the time it takes to fade to black after the boss dies
    [NonSerialized] private const byte FADE_TIME = 3;

    // the boss
    [SerializeField] private GameObject blackPanel;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = boss.GetComponent<healthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Pause.paused)
        {
            if (!bossDied)
            {
                bossBarBG.SetActive(true);

                // if we're on the boss floor and our boss isnt dead and we arent paused
                if (FloorManager.floor == 3)
                {
                    // if the boss health has changed
                    if (bossHealth.health != storedHealth)
                    {
                        // assuming the boss isnt dead
                        if (bossHealth.health > 0)
                        {
                            // scale the bar according to health over maxhealth, meaning at half health the bar will be at x scale 0.5f
                            bossBarFG.transform.localScale = new Vector3(((float)bossHealth.health / (float)bossHealth.maxHealth), 1f, 1f);
                            // update our stored health
                            storedHealth = bossHealth.health;
                        }
                        else // our boss IS dead
                        {
                            // set our boss to be dead
                            bossDied = true;
                        }

                        storedHealth = bossHealth.health;
                    }
                }
            }
            else
            {
                bossBarBG.SetActive(false);
            }
        }
        else
        {
            bossBarBG.SetActive(false);
        }
    }

    // after 4 seconds of the boss being dead, load the credits
    private void FixedUpdate()
    {
        if (bossDied && !startedFade)
        {
            Destroy(boss);

            startedFade = true;
        }
        else if (startedFade)
        {
            if (timer >= FADE_TIME)
            {
                SceneManager.LoadScene("credits");
            }
            else
            {
                timer += Time.fixedDeltaTime;

                blackPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, timer / FADE_TIME);
            }
        }
    }

    #endregion
}
