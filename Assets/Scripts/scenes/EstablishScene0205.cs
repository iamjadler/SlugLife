using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0205 : EstablishScene
{
    public GameObject slugPosition;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        if ((states.GetState(AvailableStates.meHasSlug) != "1") && (states.GetState(AvailableStates.bananaQuestStarted) == "1"))
        {
            GameObject slug = GameObject.Find("me").GetComponent<Me>().GetSlug();
            slug.transform.position = slugPosition.transform.position;
            slug.SetActive(true);
        }    
    }
}
