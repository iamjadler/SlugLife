using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : Character
{
    public GameObject slugFront;

    override protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    void DecideToJumpOnMe(int responseValue)
    {
        if (responseValue == 1)
        {
            GameObject me = GameObject.Find("me");
            me.GetComponent<Animator>().SetLayerWeight(1, 1.0f);
            states.SetState(AvailableStates.meHasSlug, "1");
            gameObject.SetActive(false);
        }
        interactionCallback(responseValue);
    }
    void HadEndGoalTalk(int responseValue)
    {
        states.SetState(AvailableStates.slugDiscussedEndGame, "1");
        interactionCallback(responseValue);
    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Slug");
        interactionDialog.SetCharacter(slugFront);


        if (states.GetState(AvailableStates.retrievedMushroom) == "1")
        {
            string fileName = "SlugThanksForMushroomDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
        }
        else if ((states.GetState(AvailableStates.talkedToBinky2ndTime) == "1") && (states.GetState(AvailableStates.slugDiscussedEndGame) != "1"))
        {
            string fileName = "SlugIDDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, HadEndGoalTalk);
        }
        else if (states.GetState(AvailableStates.bananaQuestStarted) == "1")
        {
            string fileName = "SlugDialog";
            TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
            interactionCallback = completionCallback;
            interactionDialog.CreateDialogLogic(jsonFile.text, DecideToJumpOnMe);
        }

    }
}
