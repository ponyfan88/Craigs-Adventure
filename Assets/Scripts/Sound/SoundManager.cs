/* 
 * Programmers: Jack Kennedy
 * Purpose: Manages sounds
 * Inputs: when other scripts call for a sound
 * Outputs: the sounds you hear
 */

using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables

    // a list of our sounds
    public Sound[] sounds;

    #endregion

    #region Default Methods

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>(); // add our sound as a component to our list of sounds.
            sound.source.clip = sound.clip; // grab the file source

            sound.source.volume = sound.volume; // set to sound.volume instead of 1.0f
            sound.source.pitch = 1.0f; // set to sound.pitch instead of 1.0f

            sound.source.loop = sound.loop; // loop the sound
        }
    }

    private void Update()
    {
        foreach (Sound sound in sounds)
        {
            if (Pause.paused != sound.playWhilePaused)
            // if not paused AND play while paused THEN mute
            // if paused AND dont play while paused THEN mute
            {
                // stop playing the sound
                sound.source.Stop();
            }
        }
    }

    #endregion

    #region Custom Methods

    public void Play(string name)
    {
        LogToFile.Log("playing sound " + name);
        
        // find the sound we want to play inside our list of sounds.
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        // if our sound is marked to have a randomized pitch
        if (sound.randomizePitch)
        {
            // use System.Random over UnityEngine.Random since i cant get unity's to work.
            System.Random rand = new System.Random();

            // (float)rand.NextDouble() generates a value between 0 and 1
            // we multiply that by the random pitch we want times 2 to account for our later subtraction
            // subtract by our pitch so that we not only add but subtract
            sound.source.pitch = 1.0f + (((float)rand.NextDouble() * (2 * sound.randomizePitchRange)) - sound.randomizePitchRange);
        }

        if (Pause.paused == sound.playWhilePaused)
        // if not paused AND dont play while paused THEN dont mute
        // if paused AND play while paused THEN dont mute
        {
            //play the sound
            sound.source.Play();
        }
    }

    #endregion
}
