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
    public ProjectileSpawnData[] spawnDatas; // gather all the attack patterns attached to this script
    public int index = 0; // how to order of go through
    public bool spawningAuto; // if true spawns bullets automaticly rather than manually
    public bool isSequenceRandom = false; // if set to true every attack will be randomly chosen instead of going in sequental order
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
        timer = GetSpawnData().cooldown; // sets timer to count down
        // find the player
        player = GameObject.Find("player");
        // get our current position
        pos = transform.position;
    }

    void Update()
    {
        if (timer <= 0) {
            // calculate the angle to the player from the enemy
            angle = Mathf.Rad2Deg * System.Math.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);

            velocity = (transform.position - pos); // calculate our velocity (as a vector3)
            
            pos = transform.position; // update our position
            if (spawningAuto) // if the enemy is set to automatically shoot
            {
                SpawnBullets();
                timer = GetSpawnData().cooldown;
                if (isSequenceRandom)// attacks will go of randomly if set to true
                {
                    index = Random.Range(0, spawnDatas.Length);
                }
                else // when false adds one to index and resets when it's to large of a number
                {
                    index += 1;
                    if (index >= spawnDatas.Length) index = 0;
                }
            }
        }
        timer -= Time.deltaTime; // count down till next wave of projectiles
    }

    #endregion

    #region Custom Methods

    public float[] DistributeRotations()  // evenly splits projectiles between the min and max rotation by the differnce between them
    {
        for (int i = 0; i < GetSpawnData().BulletCount; ++i) // for each bullet
        {
            float fraction = (float)i / (float)GetSpawnData().BulletCount; // fractional offset between our bullets (bullet 2/3, bullet 5/12, etc.)
            float difference = GetSpawnData().maxRotation - GetSpawnData().minRotation; // our range of where the bullet should go
            float fractionOfDiff = fraction * difference; // multiply them together
            rotations[i] = fractionOfDiff + GetSpawnData().minRotation; // add to our rotations
        }
        return rotations; // give back our rotations
    }
    public GameObject[] SpawnBullets() // spawn bullets
    {
        rotations = new float[GetSpawnData().BulletCount]; //set rotation to a list with the length of the BulletCount
        if (GetSpawnData().isRandom) RandomRotations();
        else DistributeRotations();  // calls method to distribute bullets when isRandom= false
        GameObject[] spawnedBullets = new GameObject[GetSpawnData().BulletCount];
        for (int i = 0; i < GetSpawnData().BulletCount; ++i)
        {
            spawnedBullets[i] = ProjectileManager.GetBulletFromPool(); // checks if there are enough extra disabled game objects to use

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
            else // if we arent
            {
                b.rotation = rotations[i]; // sets the rotation
            }
            b.speed = GetSpawnData().bulletspeed; // sets its speed
            b.lifetime = GetSpawnData().lifespan; // sets its life span
            b.velocity = velocity; // set our bullet velocity
            b.bulletPath = GetSpawnData().bulletPath; // set how to bullet will move 
            b.SpecialEffect = GetSpawnData().SpecialEffect; // set how much we should wiggle the bullet
            if (GetSpawnData().isNotParent) spawnedBullets[i].transform.SetParent(null); // unparents the new projectile if true
        }
        return spawnedBullets;
    }
    public float[] RandomRotations() // changes rotation to a random values between Min rotation and Max rotation and sets rotations to that
    {
        for (int i = 0; i < GetSpawnData().BulletCount; ++i) // for every bullet
        {
            rotations[i] = Random.Range(GetSpawnData().minRotation, GetSpawnData().maxRotation); // get a random rotation within our min and max rotation
        }
        return rotations;
    }

    #endregion
}