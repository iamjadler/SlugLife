using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafGuard : Character
{
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    public void AllowEntry()
    {
        Vector2 currentPosition = transform.position;
        transform.position = new Vector2(currentPosition.x, currentPosition.y + 200);
        Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
        GetComponent<BoxCollider2D>().size = new Vector2(colliderSize.x, transform.localScale.y);
        states.SetState(AvailableStates.cafeteriaOpen, "1");
    }

    private void DecideWhetherToAllowEntrance(int response)
    {
        if (response == 1)
        {
            AllowEntry();
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with CafGuard");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.cafeteriaOpen) == "1")
        {
            string fileName = "CafGuardWelcomeDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.chubbsCookieQuestStarted) == "1")
        {
            string fileName = "CafGuardMovableDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideWhetherToAllowEntrance);
        }
        else
        {
            string fileName = "CafGuardDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
