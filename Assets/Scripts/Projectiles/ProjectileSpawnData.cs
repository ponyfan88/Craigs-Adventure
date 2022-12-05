/* 
 * Programmers: Anmol Acharya
 * Purpose: create pre fabricated projectile spreads
 * Inputs: none
 * Outputs: "ProjectileSpawner" and projectile attack patternData
 */

using UnityEngine;
using BulletEffects;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProjectileSpawnData", order = 1)]

public class ProjectileSpawnData : ScriptableObject
{
    [Header("Basic")]
    public GameObject bulletResource; //this desides what projectile your gonna use
    public float minRotation, maxRotation; //set the area where projectile spawn 
    public float lifespan;// how long the bullet will stay on screen be being deactivated
    public endlifeEffect endlifeEffect; // when the projectiles lifespan ends it will perform this action
    public int BulletCount; // amount of projectiles that will be spawned
    public float cooldown;// length between shots
    public bool aimAtPlayer;// if true aims at player
    public bool isRandom; //are projectiles spawned randomly in the rotation area or consistently sperated
    [Header("Moving")]
    public Bulletpaths bulletPath; //how the bullet will move
    public float SpecialEffect;// the main value if the projectiles bullet path is not linear
    // Ex if the bullet's speed is exponetal the increase in speed will be special effect
    public float bulletspeed; //how fast the projectiles are
    public bool projectileVelocityIndependent=false;
    public Vector2 Velocity;// the velocity of the projectile 
    [Header("Parenting")]
    public Vector2 positionOffset;
    public bool isNotParent; // are the projectiles parented to the original object or not
}
