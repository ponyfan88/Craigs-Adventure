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

    // Update is called once per frame
    #endregion

    #region Default Methods

    void FixedUpdate()
    {
        a += Time.deltaTime / 10;

        transform.position = new Vector3(transform.position.x + Random.Range(-a, a), transform.position.y + Random.Range(-a, a), transform.position.z + Random.Range(-a, a));
    }

    private void OnBecameInvisible()
    {
        Debug.Log("i cannot be seen");
        killme = true;
    }

    #endregion
}
