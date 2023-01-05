using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour
{
    float a = 0;
    public bool killme = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}
