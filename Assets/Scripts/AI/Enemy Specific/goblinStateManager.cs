using UnityEngine;
public class goblinStateManager : MonoBehaviour
{
    ProjectileSpawner projectileSpawner;
    GameObject player;
    Vector2 distance;
    private void Awake()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        player = GameObject.Find("player");
    }
    public void TrySpit()
    {
        distance = player.transform.position - transform.position;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, distance);
        Debug.DrawRay(transform.position, distance, Color.red, 1f);

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
        Debug.Log("Spit");
        projectileSpawner.SpawnBullets();
    }
}
