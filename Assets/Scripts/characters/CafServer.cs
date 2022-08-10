using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafServer : Character
{

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideWhetherToGiveSandwich(int response)
    {
        if (response == 1)
        {
            states.GetObjectByName("sandwich").GetComponent<Collectable>().PickUp();
            states.SetState(AvailableStates.acquiredSandwich, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with CafServer");
        interactionDialog.SetCharacter(GameObject.Find("cafserver"));

        if (states.GetState(AvailableStates.acquiredSandwich) == "1")
        {
            string fileName = "CafServerBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else
        {
            string fileName = "CafServerGivesSandwichDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToGiveSandwich);
        }
    }
}
