/* 
 * Programmers: Jack Kennedy
 * Purpose: display health
 * Inputs: player health and if we are paused
 * Outputs: on screen hearts
 */

using System.Collections.Generic; // so we can use lists
using UnityEngine; // unity needs this in many cases
using UnityEngine.UI; // so we can manipulate images

public class Status : MonoBehaviour
{
    #region Variables

    public GameObject statusUI; // our status
    public GameObject heartsUI; // our hearts object
    public GameObject heartPrefab; // the default heart prefab
    public Sprite fullHeart; // sprite for full heart
    public Sprite halfHeart; // sprite for half heart
    public Sprite emptyHeart; // sprite for empty heart
    public Sprite missingHeart; // sprite we use when we dont set the heart (default/missing)

    int prevmaxhealth = 0; // our max health last frame
    int prevhealth = 0; // our health last frame

    int health = 0; // our health
    int maxhealth = 0; // our max health

    healthManager healthManager; // health manager variable, we need to be able to actually get our health
    FloorManager floorManager; // so we can get what floor we're on
    
    #endregion
    
    #region Default Methods
    
    void Start()
    {
        // find the health manager by first finding the player and then getting the healthManager component
        healthManager = GameObject.Find("player").GetComponent<healthManager>();

        floorManager = FindObjectOfType<FloorManager>();

        health = healthManager.health; // grab the proper health values
        maxhealth = healthManager.maxHealth;

        // offset these by -1 so that on the first frame of our program we update our hearts
        prevhealth = health - 1;
        prevmaxhealth = maxhealth - 1;
        // they will eventually be set correctly, but until then they should just be not equal to their normal values
    }

    // Update is called once per frame
    void Update()
    {
        // if we are paused
        if (Pause.paused)
        {
            // hide our status, as to avoid the pause menu and status overlapping
            statusUI.SetActive(false);
        }
        else // assuming we arent paused
        {
            // show our status
            statusUI.SetActive(true);
            
            health = healthManager.health;
            maxhealth = healthManager.maxHealth;

            // if our health/maxhealth has changed
            if (health != prevhealth || maxhealth != prevmaxhealth)
            {
                // update our hearts
                displayHearts();
            }

            // update these, the next frame they will be once again out of date (on purpose), but since we arent using them any longer it should be fine.
            prevhealth = health;
            prevmaxhealth = maxhealth;
        }
    }

    #endregion

    #region Custom Methods
    
    // function to display our hearts
    public void displayHearts()
    {
        List<Sprite> hearts = new List<Sprite>();

        for (int i = 0; i < maxhealth; i += 2)
        {
            Sprite heart = missingHeart;

            if (i == health - 1)
            {
                heart = halfHeart;
            }
            else if (i > health || i == health)
            {
                heart = emptyHeart;
            }
            else if (i < health)
            {
                heart = fullHeart;
            }

            hearts.Add(heart);
        }

        // remove previous hearts

        foreach (Transform heart in heartsUI.transform) // for every existing heart
        {
            GameObject.Destroy(heart.gameObject); // destroy the heart (we will be making new ones)
        }

        // heart offset to 0
        float heartOffset = 0f;

        // on the boss level our camera zooms out, so we need to move the hearts even more to the right than usual -
        // about doubly so

        // default for rooms 1-3
        float heartDelta = 1.4f;

        if (floorManager.floor >= 4) // if we're on the boss level
        {
            heartDelta *= 2; // multiply by 2
        }

        // add new hearts
        foreach (Sprite heart in hearts)
        {
            // clone our prefab, parent it to our heartsUI (an empty object), and make sure its position isnt all screwed up
            GameObject heartObject = Instantiate(heartPrefab, heartsUI.transform, false);

            // we need to offset our hearts by 1.4f in xpos every frame, just so they dont all create in one spot
            heartObject.transform.position = new Vector3(heartObject.transform.position.x + heartOffset, heartObject.transform.position.y, heartObject.transform.position.z); // set its position
            
            // set the sprite (half heart if we need to :))
            heartObject.GetComponent<Image>().sprite = heart;
            // enable the image component; it isnt enabled by default
            heartObject.GetComponent<Image>().enabled = true;

            // increase our offset since we just made a heart and our next heart should be to the right of that one
            heartOffset += heartDelta;
        }
    }

    #endregion
}
