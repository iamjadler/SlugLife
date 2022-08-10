using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other : Character
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
        Debug.Log("Interacting with Other");
        interactionDialog.SetCharacter(gameObject);
        string fileName = "OtherDialog";
        TextAsset jsonFile = Resources.Load(fileName) as TextAsset;
        interactionDialog.CreateDialogLogic(jsonFile.text, completionCallback);
    }
}
