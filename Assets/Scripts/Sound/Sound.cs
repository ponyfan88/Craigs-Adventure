/* 
 * Programmers: Jack Kennedy
 * Purpose: script file for each sound
 * Inputs: data set by user
 * outputs: stores sound information
 */

using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name; // the sound name

    public AudioClip clip; // the sound source

    [Range(0f, 5f)] // limit to 0% and 500%
    public float volume; // the sound volume

    public bool randomizePitch; // check this box for the sound to have a random pitch
    public float randomizePitchRange; // move this slider to change how much the sound varies

    public bool playWhilePaused; // check this box for the sound to play even while the game is paused

    public bool loop; // check this box for the sound to loop

    // the source of our sound, we hide this because its always going to be our sound manager
    [HideInInspector] // hide because changing it breaks things
    public AudioSource source; // the source (again, always the same)
}
