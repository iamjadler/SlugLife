using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : Character
{
    public GameObject slug;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideWhetherToGivePencil(int response)
    {
        if (response == 1)
        {
            GameObject.Find("pencil").GetComponent<Collectable>().PickUp();
            states.SetState(AvailableStates.acquiredPencil, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Beat");
        interactionDialog.SetCharacter(gameObject);

        if (states.GetState(AvailableStates.acquiredPencil) == "1")
        {
            string fileName = "BeatBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else
        {
            string fileName = "BeatGivesPencilDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToGivePencil);
        }
    }
}
