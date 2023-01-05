/* 
 * Programmers: Jack Kennedy
 * Purpose: display boss health
 * Inputs: boss health and if we are paused
 * Outputs: a large health bar on screen
 */

using UnityEngine;

public class BossHealth : MonoBehaviour
{
    #region Variables

    // health we store to update
    private int storedHealth = -1;

    // the boss
    [SerializeField] private GameObject boss;

    // the bars themselves
    [SerializeField] private GameObject bossBarFG;
    [SerializeField] private GameObject bossBarBG;

    // our boss health
    private healthManager bossHealth;

    // bool for if our boss has died
    private bool bossDied = false;

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
                if (FloorManager.floor == 4)
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

    #endregion
}
