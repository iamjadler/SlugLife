using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc : Character
{
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {

    }

    override public void StartInteraction(CompletionCallback completionCallback)
    {
        base.StartInteraction(completionCallback);
        Debug.Log("Interacting with Misc");
        interactionDialog.SetCharacter(gameObject);
        string fileName = "QuestionDialog";
        TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
        interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
    }
}
