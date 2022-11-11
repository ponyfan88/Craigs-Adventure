/*
 * Programmers: Jack Kennedy
 * Purpose: to move clouds from one side of the screen to the other and back
 * Inputs: none
 * Outputs: clouds, moving on screen
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    #region

    // the clouds we have in the scene
    List<cloud> clouds = new List<cloud>();

    // the cloud prefabs we can choose from (they are in-editor)
    [SerializeField] public GameObject[] cloudPrefabs;

    // various cloud related inputs, all for position
    public int cloudCount; // the number of clouds
    public int minCloudXPos; // where they start
    public int maxCloudXPos; // where they end
    public int minCloudYPos; // the highest they can be
    public int maxCloudYPos; // the lowest they can be
    public float minCloudSpeed; // the slowest the cloud can move
    public float maxCloudSpeed; // the fastest the cloud can move

    #endregion

    #region Structs

    // struct for every cloud
    struct cloud
    {
        public GameObject gameObject; // it has a gameobject
        public float startingX; // it has a starting x position
        public float speed; // it has a speed
    }

    #endregion

    #region Default Methods

    // start is called before the first update frame
    void Start()
    {
        for (int i = 0; i < cloudCount; ++i) // for each cloud we want to make
        {
            // our cloud
            cloud newCloud = new cloud();

            // instantiate the cloud as a random GameObject from our list of prefabs
            newCloud.gameObject = GameObject.Instantiate<GameObject>(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]);

            newCloud.gameObject.transform.SetParent(transform); // set its parent to the clouds object

            // get a random x position for the cloud to start at
            float cloudX = Random.Range(minCloudXPos, maxCloudXPos) - minCloudXPos;

            // move the cloud to start at that random x AND give it a random y pos
            newCloud.gameObject.transform.position = new Vector2(cloudX, Random.Range(minCloudYPos, maxCloudYPos));

            newCloud.startingX = cloudX; // set our clouds startingx to cloudx

            // set the speed to a random number between the two we chose
            newCloud.speed = Random.Range(minCloudSpeed, maxCloudSpeed);

            clouds.Add(newCloud); // add the cloud we've made to our list of clouds
        }
    }

    // update is called once per frame
    void Update()
    {
        foreach (cloud currentCloud in clouds) // for every cloud
        {
            // move the cloud, explained with this desmos: https://www.desmos.com/calculator/ldcssys13v
            currentCloud.gameObject.transform.position = new Vector2(((currentCloud.startingX + Time.time * currentCloud.speed) % (maxCloudXPos - minCloudXPos)) + minCloudXPos, currentCloud.gameObject.transform.position.y);
        }
    }

    #endregion
}