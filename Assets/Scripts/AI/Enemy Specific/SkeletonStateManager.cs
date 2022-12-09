using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateManager : MonoBehaviour
{
    ProjectileSpawner projectileSpawner;
    Animator animator;
    void Start()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
    }
    void Attack()
    {
        // Spawn bullet
        projectileSpawner.spawnerController(0);
        // Make the skeleton move again
        animator.SetBool("reachedLocation", false);
    }
}
