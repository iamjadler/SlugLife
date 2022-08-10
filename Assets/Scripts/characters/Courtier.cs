using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courtier : Character
{
    public Collectable cookies;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideToTradeCookies(int response)
    {
        if (response == 1)
        {
            GameObject cookies = states.GetObjectByName("cookies");
            GameObject sandwich = states.GetObjectByName("sandwich");
            Inventory inventory = GameObject.Find("InventoryContainer").GetComponent<Inventory>();
            Vector3 cookiesPosition = cookies.transform.position;
            cookies.GetComponent<Collectable>().PickUp();
            sandwich.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";
            sandwich.GetComponent<Collectable>().Drop(cookiesPosition);
            states.SetState(AvailableStates.acquiredCookies, "1");
            states.SetState(AvailableStates.understoodDoubt, "1");
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Courtier");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.acquiredCookies) == "1")
        {
            string fileName = "CourtierBlabber2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.acquiredSandwich) == "1")
        {
            string fileName = "CourtierTradeForCookiesDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToTradeCookies);
        }
        else if (states.GetState(AvailableStates.chubbsCookieQuestStarted) == "1")
        {
            string fileName = "CourtierNoSandwichDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else
        {
            string fileName = "CourtierDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
