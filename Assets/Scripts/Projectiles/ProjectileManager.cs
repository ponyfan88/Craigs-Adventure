/* 
 * Programmers: Anmol Acharya
 * Purpose: reuse projectile
 * Inputs: "bulletSpawner"
 * Outputs: reused projectiles
 */

using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    #region Variables

    public static List<GameObject> bullets;// creates a list of all the objects in the scene

    #endregion

    #region Default Methods

    private void Start()
    {
        bullets = new List<GameObject>();// sets bullet to that list
    }

    #endregion

    #region Custom Methods

    public static GameObject GetBulletFromPool(string name)
/* Purpose: reuse projectile to prevent a memory leak
 * Inputs: name of projectile to look for
 * Outputs: projectile spawner
 */
    { 
        for (int i = 0; i < bullets.Count; ++i)
        {
            if (!bullets[i].activeSelf&& bullets[i].name==name) // resues old objects instead of insatiating more objects
            {
                bullets[i].GetComponent<Projectile>().ResetTimer();//gets bullet and resets it's timer
                bullets[i].SetActive(true); //set it active
                return bullets[i];//returns unused bullets
            }
        }
        return null;// returns null if there is no bullets to reuse
    }

    #endregion
}
