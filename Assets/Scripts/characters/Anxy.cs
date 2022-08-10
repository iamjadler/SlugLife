using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anxy : Character
{

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    private void DecideWhetherIRemember(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.helpedAnxyRemember, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Anxy");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.acquiredId41) == "1")
        {
            string fileName = "AnxyBlabber3Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.helpedAnxyRemember) == "1")
        {
            string fileName = "AnxyBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.pencilQuestStarted) == "1")
        {
            string fileName = "AnxyRemembersDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherIRemember);
        }
        else
        {
            string fileName = "AnxyDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
