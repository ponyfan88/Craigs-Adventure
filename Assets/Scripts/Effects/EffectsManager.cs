/* 
 * Programmers: Jack Kennedy
 * Purpose: Manages effects
 * Inputs: when other scripts call for an effect
 * Outputs: on screen effects
 */

using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{

    #region Variables

    // these are the objects we'll run effects on
    public List<effectItemProperties> effectsObjects = new List<effectItemProperties>();
    // these are the objects we'll get rid of from effectsObjects every frame; it's cleared every frame
    public List<int> deletedObjects = new List<int>();

    #endregion

    #region Structs

    // every effectsObject gets its own data type, this is all referenced above
    public struct effectItemProperties
    {
        public GameObject gameObject;
        public GlobalFX.effect effect;
        public float time;
        public Color color;
        public Color startingColor;
        public float amount;
        public float timeStarted;
        public float repeat;
    }

    #endregion

    #region Default Methods

    public void Update()
    {
        // dont delete any more objects; they were deleted last frame
        deletedObjects = new List<int>();
        // for every effectsObject
        for (int i = 0; i < effectsObjects.Count; i++)
        {
            if (Time.time >= effectsObjects[i].time + effectsObjects[i].timeStarted) // effect has expired
            {
                // make our object look normal
                try
                {
                    effectsObjects[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                catch
                {
                    LogToFile.Log("object " + i.ToString() + " was likely destroyed, so we skip resetting the color of it.");
                }

                // no matter what, EVEN IF THE OBJECT WAS DELETED we need to remove it from our list.

                deletedObjects.Add(i); // add the index of the object to delete

                LogToFile.Log("effect object " + i.ToString() + "has expired");
            }
            else
            {
                // used with every particle so might as well calculate now
                // im using jmath.mod for like no reason idk
                // TODO: USE NORMAL MOD
                float timeProgress = JMath.mod(((Time.time - effectsObjects[i].timeStarted) / (effectsObjects[i].time * effectsObjects[i].repeat)) * Mathf.Pow(effectsObjects[i].repeat, 2), 1f);

                try
                {
                    // we just test against every known effect. these are made in globalfx.
                    switch (effectsObjects[i].effect)
                    {
                        case GlobalFX.effect.flash:
                            // flash
                            float colorAmount = (Mathf.Sin(((timeProgress - (0.25f * effectsObjects[i].time)) * Mathf.PI * 2f) / effectsObjects[i].time) + 1f) / 2f;
                            colorAmount *= effectsObjects[i].amount; // multiply by amount
                            Color flashColor = betweenColor(effectsObjects[i].startingColor, effectsObjects[i].color, colorAmount);
                            effectsObjects[i].gameObject.GetComponent<SpriteRenderer>().color = flashColor;
                            break;
                        case GlobalFX.effect.flashTransparent:
                            // flashTransparent
                            // explination https://www.desmos.com/calculator/1qnkncfggh
                            float transparencyAmount = 1.0f - (Mathf.Sin(((((timeProgress - (0.25f * effectsObjects[i].time)) * Mathf.PI * 2f) / effectsObjects[i].time)) + 1f) / 2f);
                            transparencyAmount *= effectsObjects[i].amount; // multiply by amount
                            Color flashTransparencyColor = new Color(effectsObjects[i].color.r, effectsObjects[i].color.g, effectsObjects[i].color.b, transparencyAmount);
                            effectsObjects[i].gameObject.GetComponent<SpriteRenderer>().color = flashTransparencyColor;
                            break;
                        default:
                            // we obviously havent supplied a valid effect, might as well say so
                            LogToFile.Log("MISSING EFFECT " + effectsObjects[i].effect.ToString());
                            break;
                    }
                }
                catch // this means our object was probably an enemy, and that enemy was probably destroyed
                {
                    LogToFile.Log("object " + i.ToString() + " is destroyed or bugged, adding to deleted objects.");

                    deletedObjects.Add(i); // add the index of the object to delete
                }
            }
        }

        int offset = 0;

        // delete every object no longer in use from our list
        foreach (int num in deletedObjects) // deletedObjects is a list of item numbers
        {
            effectsObjects.RemoveAt(num - offset); // we use removeat for each item in deletedobjects
            ++offset; // this should always work, as the first item in num should always be smaller than any above it.

            LogToFile.Log("effect object " + num.ToString() + "has been deleted"); // log the number we just deleted
        }
    }

    #endregion

    #region Custom Methods
    // method to addeffect
    // we take the game object to act upon
    // we get an effect to put on that object (struct is in globalFX.cs)
    // we get the time that the effect runs for
    // the color of our effect (by default we do magenta since its very easy to see when effects are missing)
    // we get our amount (this isnt even used yet) //TODO: USE THIS
    // we get the number of times to repeat the effect
    public void addEffect(GameObject gameObject, GlobalFX.effect effect, float time = 5f, Color? color = null, float amount = 1f, float repeat = 1f)
    {
        effectItemProperties effectsObject = new effectItemProperties();
        effectsObject.gameObject = gameObject;
        effectsObject.effect = effect;
        effectsObject.time = time;
        effectsObject.color = color ?? new Color(1f, 0f, 1f); // if no color is supplied, do magenta
        effectsObject.amount = amount;
        effectsObject.timeStarted = Time.time; // when we started the effect; used to calculate our progress
        effectsObject.repeat = repeat;
        effectsObject.startingColor = gameObject.GetComponent<SpriteRenderer>().color; // the color of the object we are acting upon
        // as of right now we only support the SpriteRenderer, in the future we may make staritng color an input, its just really embarassing to have to do that

        effectsObjects.Add(effectsObject);
    }

    // this function returns a color between color1 and color2 on a scale of 0-1 (0 being color1 and 1 being color2)
    private Color betweenColor(Color color1, Color color2, float amount = 0.5f)
    {
        return new Color(color1.r + (color2.r - color1.r) * amount, color1.g + (color2.g - color1.g) * amount, color1.b + (color2.b - color1.b) * amount);
    }
    
    #endregion
}