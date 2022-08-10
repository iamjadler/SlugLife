using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTroll : Character
{
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    public void MoveToNewSpot()
    {
        transform.position = GameObject.Find("bridgeTrollMovedSpot").transform.position;
        Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
        GetComponent<BoxCollider2D>().size = new Vector2(transform.localScale.x, colliderSize.y);
    }


    private void DecideWhetherToAllowCrossing(int response)
    {
        if (response == 1)
        {
            MoveToNewSpot();
            states.SetState(AvailableStates.bridgeTrollMoved, "1");
        }
        interactionCallback(response);
    }

    private void DecideWhetherToLearnAboutPeerPressure(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.learnedAboutPeerPressure, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with BridgeTrollGuard");
        interactionDialog.SetCharacter(gameObject);

        if (states.GetState(AvailableStates.bridgeTrollMoved) == "1")
        {
            string fileName = "BridgeTrollBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if ( (states.GetState(AvailableStates.angieMoved) == "1") && (states.GetState(AvailableStates.chubbsMoved) == "1") && (states.GetState(AvailableStates.doubtMoved) == "1") )
        {
            string fileName = "BridgeTrollMovesDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToAllowCrossing);
        }
        else if (states.GetState(AvailableStates.talkedToBinky2ndTime) == "1")
        {
            string fileName = "BridgeTrollPeerPressureDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToLearnAboutPeerPressure);
        }
        else
        {
            string fileName = "BridgeTrollDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
