/*
 * Programmers: Jack Kennedy
 * Purpose: controls the credits, making the end text appear
 * Inputs: the positions of the credits
 * Outputs: shows the "end" message
 */

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private GameObject anmol, xander, chris, jack, end;

    [NonSerialized] private Name anmolName, xanderName, chrisName, jackName;

    [NonSerialized] private bool anmolDestroyed = false, xanderDestroyed = false, chrisDestroyed = false, jackDestroyed = false;

    [NonSerialized] private float endTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // grab all the names of each developer

        anmolName = anmol.GetComponent<Name>();
        xanderName = xander.GetComponent<Name>();
        chrisName = chris.GetComponent<Name>();
        jackName = jack.GetComponent<Name>();
    }

    // Update is called once per frame
    void Update()
    {
        // test for each name and if its destroyed
        if (!anmolDestroyed)
        {
            // if its offscreen
            if (anmolName.killme)
            {
                // destroy the name
                Destroy(anmol);

                // set the destroyed variable to true
                anmolDestroyed = true;
            }
        }

        if (!xanderDestroyed)
        {
            if (xanderName.killme)
            {
                Destroy(xander);
                xanderDestroyed = true;
            }
        }

        if (!chrisDestroyed)
        {
            if (chrisName.killme)
            {
                Destroy(chris);
                chrisDestroyed = true;
            }
        }

        if (!jackDestroyed)
        {
            if (jackName.killme)
            {
                Destroy(jack);
                jackDestroyed = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // if everyone is destroyed
        if (anmolDestroyed && xanderDestroyed && chrisDestroyed && jackDestroyed)
        {
            // increment the timer
            endTimer += Time.fixedDeltaTime;
        }

        // once the timer is at 1 second
        if (endTimer >= 1)
        {
            // display the end text
            end.SetActive(true);
        }

        // once the timer is at 5 seconds
        if (endTimer >= 5)
        {
            // load the main menu
            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }
}
