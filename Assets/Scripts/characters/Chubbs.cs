using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chubbs : Character
{

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }
    public void MoveToNewSpot()
    {
        transform.position = GameObject.Find("chubbsMovedSpot").transform.position;
    }

    void DecideToGiveCookie(int response)
    {
        if (response == 1)
        {
            MoveToNewSpot();
            GameObject cookies = states.GetObjectByName("cookies");
            cookies.GetComponent<SpriteRenderer>().sortingLayerName = "collectibles";
            cookies.GetComponent<Collectable>().Drop(transform.position);
            states.SetState(AvailableStates.chubbsMoved,"1");
        }
        interactionCallback(response);
    }

    void DecideToStartCookieQuest(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.chubbsCookieQuestStarted, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Chubbs");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.chubbsMoved) == "1")
        {
            string fileName = "ChubbsBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.acquiredCookies) == "1")
        {
            string fileName = "ChubbsGetsCookieDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToGiveCookie);
        }
        else if (states.GetState(AvailableStates.learnedAboutPeerPressure) == "1")
        {
            string fileName = "ChubbsAsks4CookieDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToStartCookieQuest);
        }
        else
        {
            string fileName = "ChubbsDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
