using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binky : Character
{
    public GameObject slugPosition;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideWhetherPencilTalkWasDone(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.talkedToBinkyAboutPencil, "1");
        }
        interactionCallback(response);
    }

    void DecideWhether2ndTalkWasDone(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.talkedToBinky2ndTime, "1");
        }
        interactionCallback(response);
    }

    void DecideToAllowSlugToAppear(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.bananaQuestStarted, "1");
            GameObject slug = GameObject.Find("me").GetComponent<Me>().GetSlug();
            slug.transform.position = slugPosition.transform.position;
            slug.SetActive(true);
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Binky");
        interactionDialog.SetCharacter(gameObject);

        if (states.GetState(AvailableStates.acquiredPencil) == "1")
        {
            string fileName = "BinkyBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.pencilQuestStarted) == "1")
        {
            string fileName = "BinkyPencilDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherPencilTalkWasDone);
        }
        else if (states.GetState(AvailableStates.meHasSlug) == "1")
        {
            string fileName = "BinkySlugDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhether2ndTalkWasDone);
        }
        else
        {
            string fileName = "BinkyDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToAllowSlugToAppear);
        }
    }
}
