using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote : Character
{
    public Sprite emoteWithHat;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void DecideToEndGame(int response)
    {
        if (response == 1)
        {
            states.SetState(AvailableStates.missionCompleted, "1");
            GetComponent<SpriteRenderer>().sprite = emoteWithHat;
            GameObject me = GameObject.Find("me");
            me.GetComponent<Animator>().SetLayerWeight(1, 0.0f);
        }
        interactionCallback(response);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Emote");
        interactionDialog.SetCharacter(gameObject);
        if (states.GetState(AvailableStates.missionCompleted) == "1")
        {
            string fileName = "EmoteUseless2Dialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if (states.GetState(AvailableStates.retrievedMushroom) == "1")
        {
            string fileName = "EmoteCommunicatesDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToEndGame);
        }
        else if (states.GetState(AvailableStates.id42CardRetrievedFromBeach) == "1")
        {
            string fileName = "EmoteUselessDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else
        {
            string fileName = "EmoteDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
    }
}
