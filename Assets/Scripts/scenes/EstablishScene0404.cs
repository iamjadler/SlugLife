using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0404 : EstablishScene
{
    public GameObject emote;
    public Sprite emoteWithHat;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        if (states.GetState(AvailableStates.missionCompleted) == "1")
        {
            emote.GetComponent<SpriteRenderer>().sprite = emoteWithHat;
        }
    }
}
