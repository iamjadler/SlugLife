using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Collectable
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void PickUp(bool animate = true, AnimationCompleteCallback callback = null)
    {
        base.PickUp(animate, callback);
        states.SetState(AvailableStates.id42CardRetrievedFromBeach, "1");
    }
}
