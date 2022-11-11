/*
 * Programmers: Christopher Kowalewski
 * Purpose: used to detect possible collisions for determining if a 2 by 2 room can spawn or if it would overlap anything.
 * Inputs: None, besides being spawned by a spawnpoint attempting to spawn a 2 by 2 room.
 * Outputs: conflict bool, determines if anything was collided with (if true, a 2 by 2 won't spawn)
 */

using UnityEngine;

public class testSpawn : MonoBehaviour
{
    /*
     * this will go in the testSpawn spawnpoint object
     * when it collides with ANYTHING AT ALL that probably means a multi-sized room (2by2, 2x1, etc.) cannot spawn.
     * therefore if it does not, return/set a value signifying that everything is fine.
     * rGen will combine the data and determine whether to spawn a regular 1x1 or a multi-sized room (2by2, 2x1, etc.)
     */

    #region Variables
    
    public bool conflict = false;
    
    #endregion

    #region Default Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        conflict = true;
    }

    #endregion
}
