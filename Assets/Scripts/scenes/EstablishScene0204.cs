using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0204 : EstablishScene
{
    public GameObject mushroom;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        if ((states.GetState(AvailableStates.treeClimbed) == "1") && (states.GetState(AvailableStates.retrievedMushroom) != "1"))
        {
            mushroom.SetActive(true);
        }
    }
}
