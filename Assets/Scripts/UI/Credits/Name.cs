/*
 * Programmers: Jack Kennedy
 * Purpose: controls the credits, run for every name
 * Inputs: none
 * Outputs: moves each name
 */

using System;
using UnityEngine;

public class Name : MonoBehaviour
{
    #region Variables

    [NonSerialized] private float a = 0;
    [NonSerialized] private float timer = 0;
    [NonSerialized] public bool killme = false;

    #endregion

    #region Default Methods

    void FixedUpdate()
    {
        if (timer >= 3)
        {
            a += Time.deltaTime / 5;

            transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-a, a), transform.position.y + UnityEngine.Random.Range(-a, a), transform.position.z + UnityEngine.Random.Range(-a, a));
        }
        else
        {
            timer += Time.fixedDeltaTime;
        }
    }

    // once we are offscreen
    private void OnBecameInvisible()
    {
        // set killme to true
        killme = true;
    }

    #endregion
}
