using UnityEngine;
public class goblinStateManager : MonoBehaviour
{
    ProjectileSpawner projectileSpawner;
    getCloseToPlayer distance;
    private void Awake()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        distance = GetComponent<Animator>().GetBehaviour<getCloseToPlayer>();
    }
    public void TrySpit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, distance.distance);
        Debug.DrawRay(transform.position, distance.distance, Color.red, 100f);

        if (ray.collider.name != "player" && ray.collider.transform != transform && ray.collider.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision"))
        {
            // Logic to avoid walls
        }
        else
        {
            Spit();
        }
    }
    public void Spit()
    {
        projectileSpawner.SpawnBullets();
    }
}
