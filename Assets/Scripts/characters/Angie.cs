using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angie : Character
{

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideToStartPencilQuest(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.pencilQuestStarted, "1");
        }
        interactionCallback(response);
    }

    void DecideToSupplyId41(int response)
    {
        if (response == 1)
        {
            GameObject id41 = GameObject.Find("id41");
            id41.transform.position = GameObject.Find("id41Spot").transform.position;
            id41.GetComponent<Collectable>().PickUp();
            states.SetState(AvailableStates.acquiredId41, "1");
        }
        interactionCallback(response);
    }

    public void MoveToNewSpot()
    {
        transform.position = GameObject.Find("angieMovedSpot").transform.position;
    }

    void DecideToAcquirePencil(int response)
    {
        if (response == 1)
        {
            MoveToNewSpot();
            states.GetObjectByName("pencil").GetComponent<Collectable>().Drop(transform.position);
            states.SetState(AvailableStates.angieMoved, "1");
        }
        interactionCallback(response);
    }



    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Angie");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.angieMoved) == "1")
        {
            string fileName = "AngieBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.acquiredPencil) == "1")
        {
            string fileName = "AngieMovesDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToAcquirePencil);
        }
        else if (states.GetState(AvailableStates.acquiredId41) == "1")
        {
            string fileName = "AngieWaitingForPencilDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if ((states.GetState(AvailableStates.helpedAnxyRemember) == "1") && (states.GetState(AvailableStates.talkedToBinkyAboutPencil) == "1"))
        {
            string fileName = "AngieSupplyId41Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToSupplyId41);
        }
        else if (states.GetState(AvailableStates.pencilQuestStarted) == "1")
        {
            string fileName = "AngieWIPQuestDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.learnedAboutPeerPressure) == "1")
        {
            string fileName = "AngieStartPencilQuestDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToStartPencilQuest);
        }
        else
        {
            string fileName = "AngieDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
