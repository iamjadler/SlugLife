using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doubt : Character
{

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    public void MoveToNewSpot()
    {
        transform.position = GameObject.Find("doubtMovedSpot").transform.position;
    }

    private void DecideWhetherToMove(int response)
    {
        if (response == 1)
        {
            MoveToNewSpot();
            states.SetState(AvailableStates.doubtMoved, "1");
        }
        interactionCallback(response);
    }


    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Doubt");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.doubtMoved) == "1")
        {
            string fileName = "DoubtBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if ((states.GetState(AvailableStates.understoodDoubt) == "1") && (states.GetState(AvailableStates.chubbsMoved) == "1") && (states.GetState(AvailableStates.angieMoved) == "1"))
        {
            string fileName = "DoubtMovesDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToMove);
        }
        else if (states.GetState(AvailableStates.acquiredId41) == "1")
        {
            string fileName = "DoubtDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.talkedToBinkyAboutPencil)=="1")
        {
            string fileName = "DoubtPencilDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else
        {
            string fileName = "DoubtDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
