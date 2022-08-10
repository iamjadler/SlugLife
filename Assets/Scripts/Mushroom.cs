using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable
{
    AnimationCompleteCallback animationCompleteCallback;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void PickUp(bool animate = true, AnimationCompleteCallback callback = null)
    {
        animationCompleteCallback = callback;
        base.PickUp(animate, MushroomPickupAnimationComplete);
    }

    private void MushroomPickupAnimationComplete()
    {
        states.SetState(AvailableStates.retrievedMushroom, "1");
        GameObject.Find("me").GetComponent<Me>().GetSlug().GetComponent<Slug>().StartInteraction(x => { });
        if (animationCompleteCallback != null) { animationCompleteCallback(); }
    }
}
