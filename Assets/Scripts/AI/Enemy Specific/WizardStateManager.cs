/* Programmer: Xander Mooney
 * Purpose: Control specifics of when the wizard should do its behaviours
 * Inputs: the conditions of the AI
 * Outputs: what the AI should do
 */

using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(AIManager))]
public class WizardStateManager : MonoBehaviour
{
    #region Variables
    NavMeshAgent ai;
    Animator animator;
    GameObject player;

    float maxTPFromPlayer = 8;

    #endregion

    #region Default Methods

    void Awake()
    {
        player = GameObject.Find("player");
        ai = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    #endregion

    #region Custom Methods

    void InitializeTeleport()
    {
        animator.SetBool("reachedLocation", false);
        // Make the wizard fade out and back in
        EffectsManager.AddEffect(gameObject, GlobalFX.effect.flashTransparent, 1, Color.white, 1, 1f);
        // Trigger the teleport to happen as the wizard is fully transparent
        Invoke("Teleport", 0.5f);
    }
    void Teleport()
    {
        NavMeshHit hit = new NavMeshHit();
        // Make sure that the wizard is on the nav mesh; this is a failsafe to prevent infinite loops
        if (ai.isOnNavMesh)
        {
            // we always need to calculate the loop atleast once
            do
            {
                // Select a random point within a radius of the player
                Vector2 randomPoint = (Vector2)player.transform.position + Random.insideUnitCircle * maxTPFromPlayer;
                // Sample that position on the navmesh to check if its valid, if not the loop will run again
                NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas);
            } 
            while (!hit.hit);

            // Teleport the wizard
            transform.position = hit.position;
            animator.SetBool("reachedLocation", true);
        }
    }

    #endregion
}
