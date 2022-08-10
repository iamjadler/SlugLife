using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private GameObject interactionContainer;
    protected InteractionDialog interactionDialog;
    private CompletionCallback completionCallback;
    protected States states;
    protected CompletionCallback interactionCallback;
    private string teststring;

    virtual protected void Awake()
    {
        // Debug.Log("Character awake");
        teststring = gameObject.name;
        states = GameObject.Find("States").GetComponent<States>();
        interactionContainer = GameObject.Find("InteractionContainer");
        interactionDialog = interactionContainer.GetComponent<InteractionDialog>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Debug.Log("Character start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void StartInteraction(CompletionCallback completionCallback)
    {
        Debug.Log("Generic Interaction");
        interactionDialog.Activate();
    }
}
