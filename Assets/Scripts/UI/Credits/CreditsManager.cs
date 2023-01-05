using UnityEngine;
using System;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private GameObject anmol, xander, chris, jack, end;

    [NonSerialized] private Name anmolName, xanderName, chrisName, jackName;

    [NonSerialized] private bool anmolDestroyed = false, xanderDestroyed = false, chrisDestroyed = false, jackDestroyed = false;

    [NonSerialized] private float endTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        anmolName = anmol.GetComponent<Name>();
        xanderName = xander.GetComponent<Name>();
        chrisName = chris.GetComponent<Name>();
        jackName = jack.GetComponent<Name>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!anmolDestroyed)
        {
            if (anmolName.killme)
            {
                Destroy(anmol);
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

        if (anmolDestroyed && xanderDestroyed && chrisDestroyed && jackDestroyed)
        {
            endTimer += Time.deltaTime;
        }

        if (endTimer >= 1)
        {
            end.SetActive(true);
        }
    }
}
