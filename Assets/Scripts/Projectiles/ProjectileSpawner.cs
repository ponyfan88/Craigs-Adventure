/* 
 * Programmers: Anmol Acharya and Jack Kennedy
 * Purpose: spawn projectile
 * Inputs: projectile data
 * Outputs: bullets spawned
 */

using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region Variables

    GameObject player; // initialize a variable to represent player
    Vector3 pos, velocity; // initialize pos and velocity
    public ProjectileAttackPatternData[] attackPattern;//list of all attack patterns
    private ProjectileSpawnData[] spawnDatas; // list of the spawn data for current attack pattern
    public int index = 0; //the current projectile data
    bool spawning=false; // are the projectiles spawning
    double angle = 0; // angle to the player
    float timer; // countdown till next wave of projectiles
    float[] rotations; // the angle of the projectile
    ProjectileSpawnData GetSpawnData() // grab our spawn data
    {
        return spawnDatas[index];
    }

    #endregion

    #region Default Methods

    void Start()
    {
        // find the player
        player = GameObject.Find("player");
        // get our current position
        pos = transform.position;
    }

    void Update()
    {
        if (timer <= 0&& spawning) {
            // calculate the angle to the player from the enemy
            angle = Mathf.Rad2Deg * System.Math.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            pos = transform.position; // update our position
            if (!GetSpawnData().projectileVelocityIndependent) velocity = (transform.position - pos); // calculate our velocity (as a vector3)
            else velocity = new Vector2(GetSpawnData().Velocity.x, GetSpawnData().Velocity.y);//take velocity from spawnData
           
            SpawnBullets();
            timer = GetSpawnData().cooldown;
            ++index;
            if (index >= spawnDatas.Length) 
            {
                index = 0;
                spawning= false;
            }
        }
        timer -= Time.deltaTime; // count down till next wave of projectiles
    }

    #endregion

    #region Custom Methods
    /* Purpose: enable this script
 * Inputs:index of attack you wish to preform
 * Outputs: Spawn bullets
 */
    public void spawnerController(int attackIndex) 
    {
        spawning= true;
        spawnDatas = attackPattern[attackIndex].pattern;
        timer = GetSpawnData().cooldown; // sets timer to count down
    }
/* Purpose: evenly splits projectiles between the min and max rotation by the differnce between them
 * Inputs: bullet count, min and max rotation,
 * Outputs: SpawnBullets()  
 */
    public float[] DistributeRotations()      {
        for (int i = 0; i < GetSpawnData().BulletCount; ++i) // for each bullet
        {
            float fraction = (float)i / (float)GetSpawnData().BulletCount; // fractional offset between our bullets (bullet 2/3, bullet 5/12, etc.)
            float difference = GetSpawnData().maxRotation - GetSpawnData().minRotation; // our range of where the bullet should go
            float fractionOfDiff = fraction * difference; // multiply them together
            rotations[i] = fractionOfDiff + GetSpawnData().minRotation; // add to our rotations
        }
        return rotations; // give back our rotations
    }
/* Spawn bullets
 * Inputs: DistributeRotations(), isRandom,
 * Outputs: Projectile.cs  
 */
    private GameObject[] SpawnBullets() 
    {
        rotations = new float[GetSpawnData().BulletCount]; //set rotation to a list with the length of the BulletCount
        if (GetSpawnData().isRandom) RandomRotations();
        else DistributeRotations();  // calls method to distribute bullets when isRandom= false
        GameObject[] spawnedBullets = new GameObject[GetSpawnData().BulletCount];
        for (int i = 0; i < GetSpawnData().BulletCount; ++i)
        {
            spawnedBullets[i] = ProjectileManager.GetBulletFromPool(GetSpawnData().bulletResource.name); // checks if there are enough extra disabled game objects to use

            if (spawnedBullets[i] == null)
            {
                // creates new objects is there is not enough projectiles
                spawnedBullets[i] = Instantiate(GetSpawnData().bulletResource, transform);
                ProjectileManager.bullets.Add(spawnedBullets[i]);
            }
            else
            {
                spawnedBullets[i].transform.SetParent(transform); // sets unused bullets to "spawnedBullets" array
            }
            spawnedBullets[i].transform.localPosition = Vector2.zero; // set the position of the projectile to where the spawner is
            Projectile b = spawnedBullets[i].GetComponent<Projectile>(); // variable that grabs the newly spawned bullets 

            // if we are supposed to aim at the player
            if (GetSpawnData().aimAtPlayer)
            {
                b.rotation = rotations[i] + (float)angle; // rotation and aim at player
            }
            else
            {
                b.rotation = rotations[i]; // sets the rotation
            }
            b.transform.position= new Vector3(GetSpawnData().positionOffset.x+transform.position.x, GetSpawnData().positionOffset.y + transform.position.y);//offsets the bullets position
            b.speed = GetSpawnData().bulletspeed; // sets its speed
            b.lifetime = GetSpawnData().lifespan; // sets its life span
            b.velocity = velocity; // set our bullet velocity
            b.bulletPath = GetSpawnData().bulletPath; // set how to bullet will move 
            b.SpecialEffect = GetSpawnData().SpecialEffect; // set how much we should wiggle the bullet
            if (GetSpawnData().isNotParent) spawnedBullets[i].transform.SetParent(null); // unparents the new projectile if true
        }
        return spawnedBullets;
    }
/* Purpose: randomize where bullets will show up
 * Inputs: bullet count, min max rotations
 * Outputs: spawn bullet
 */
    public float[] RandomRotations()
 {
        for (int i = 0; i < GetSpawnData().BulletCount; ++i) // for every bullet
        {
            rotations[i] = Random.Range(GetSpawnData().minRotation, GetSpawnData().maxRotation); // get a random rotation within our min and max rotation
        }
        return rotations;
    }

    #endregion
}