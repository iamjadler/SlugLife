using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0105 : EstablishScene
{
    public GameObject lostOnBeachLocation;
    public GameObject bridgeTroll;
    public GameObject angie;
    public GameObject chubbs;
    public GameObject doubt;

    // Start is called before the first frame update
    override public void Start()
    {
        Inventory inventory = GameObject.Find("InventoryContainer").GetComponent<Inventory>();

        base.Start();
        if (GameObject.Find("me").GetComponent<Me>().GetScaleFactor() == 1.0f)
        {
            GameObject.Find("LowerWalls").SetActive(false);
        }
        else
        {
            GameObject.Find("UpperWalls").SetActive(false);
            GameObject.Find("bridge").GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";
        }

        if (inventory.DoesExist("id42") && (states.GetState(AvailableStates.id42CardRetrievedFromBeach) != "1"))
        {
            states.GetObjectByName("id42").GetComponent<Collectable>().Drop(lostOnBeachLocation.transform.position, false);
        }

        if ((states.GetState(AvailableStates.slugDiscussedEndGame)!="1") && (states.GetState(AvailableStates.meHasSlug) == "1") && (states.GetState(AvailableStates.talkedToBinky2ndTime)=="1"))
        {
            GameObject.Find("me").GetComponent<Me>().GetSlug().GetComponent<Slug>().StartInteraction( x => { } );
        }

        if (states.GetState(AvailableStates.bridgeTrollMoved) == "1")
        {
            bridgeTroll.GetComponent<BridgeTroll>().MoveToNewSpot();
        }
        if (states.GetState(AvailableStates.angieMoved) == "1")
        {
            angie.GetComponent<Angie>().MoveToNewSpot();
        }
        if (states.GetState(AvailableStates.chubbsMoved) == "1")
        {
            chubbs.GetComponent<Chubbs>().MoveToNewSpot();
        }
        if (states.GetState(AvailableStates.doubtMoved) == "1")
        {
            doubt.GetComponent<Doubt>().MoveToNewSpot();
        }
    }

}
