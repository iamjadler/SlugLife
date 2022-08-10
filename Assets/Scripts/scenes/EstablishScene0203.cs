using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0203 : EstablishScene
{
    public GameObject cafGuard;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        if (states.GetState(AvailableStates.cafeteriaOpen) == "1")
        {
            cafGuard.GetComponent<CafGuard>().AllowEntry();
        }
    }
}
