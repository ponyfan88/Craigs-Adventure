/*
 * Programmers: Jack Kennedy
 * Purpose: controls the credits, run for every name
 * Inputs: none
 * Outputs: moves each name
 */

using UnityEngine;

public class Name : MonoBehaviour
{
    #region Variables

    float a = 0;
    public bool killme = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    #endregion

    #region Default Methods

    void FixedUpdate()
    {
        // move randomly every frame :)

        a += Time.deltaTime / 10;

        transform.position = new Vector3(transform.position.x + Random.Range(-a, a), transform.position.y + Random.Range(-a, a), transform.position.z + Random.Range(-a, a));
    }

    // once we are offscreen
    private void OnBecameInvisible()
    {
        Debug.Log("i cannot be seen");
        // set killme to true
        killme = true;
    }

    #endregion
}
