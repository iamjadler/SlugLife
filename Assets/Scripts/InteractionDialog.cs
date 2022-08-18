using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public delegate void CompletionCallback(int responseValue);

[System.Serializable]
class DialogChoice
{
    public string text;
    public int action;
}

[System.Serializable]
class Dialog
{
    public int selection;
    public string text;
    public List<DialogChoice> choices;
    public int action;
}

[System.Serializable]
class DialogLogic
{
    public List<Dialog> dialogs;
}

public class InteractionDialog : MonoBehaviour
{
    public GameObject interactionCanvas;
    public GameObject interactionDialogBox;
    public GameObject interactionImage;
    public GameObject interactionInstructions;
    public List<Sprite> dialogSpriteList;
    public List<GameObject> choiceButtons;
    public Sprite meFrontSprite;
    private DialogLogic dialogLogic;
    private List<KeyCode> acceptableKeypresses = new();
    private Dialog currentDialog;
    private CompletionCallback completionCallback;
    private bool allowInteraction = false;
    private bool skipDialog = false;
    private SurveyDialog surveyDialog;
    private List<int> surveyDialogsAlreadyShown = new();
    private List<KeyCode> keypressesThatCantExitDialog = new();
    private AudioSource newSong;
    private bool simpleDialog = false;
    private string touchString = "";
    private DialogTypes currentDialogType = DialogTypes.noDialog;
    private Sprite defaultCharacter;

    enum DialogTypes
    {
        noDialog,
        npcDialog,
        responseDialog,
        surveyDialog
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        surveyDialog = GameObject.Find("SurveyContainer").GetComponent<SurveyDialog>();
        keypressesThatCantExitDialog.Add(KeyCode.UpArrow);
        keypressesThatCantExitDialog.Add(KeyCode.DownArrow);
        keypressesThatCantExitDialog.Add(KeyCode.LeftArrow);
        keypressesThatCantExitDialog.Add(KeyCode.RightArrow);
    }

    bool HaveAnyOfTheseKeysBeenPressed(List<KeyCode> keypresses)
    {
        foreach (KeyCode keycode in keypresses)
        {
            if (Input.GetKeyDown(keycode)) { return true; }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInteraction)
        {
            if (skipDialog)
            {
                skipDialog = false;
                SetupNextDialog(0);
            }
            else if ( (Input.GetKeyDown(KeyCode.X) || (touchString == "Exit")) && ((currentDialog.action > 0) || (acceptableKeypresses.Count > 0)))
            {
                // user pressed 'X' to quit dialog, AND next action wasn't going to exit anyway
                AllDone(0);
            }
            else if (acceptableKeypresses.Count == 0)
            {
                if (Input.anyKeyDown || (touchString == "Ok"))
                {
                    if (!HaveAnyOfTheseKeysBeenPressed(keypressesThatCantExitDialog))
                    {
                        if (simpleDialog)
                        {
                            AllDone(0);
                        }
                        else
                        {
                            SetupNextDialog(0);
                        }
                    }
                }
            }
            else if (touchString.Length > 0)
            {
                SetupNextDialog(int.Parse(touchString));
            }
            else
            {
                foreach (int keycode in acceptableKeypresses)
                {
                    if (Input.GetKeyDown((KeyCode)keycode))
                    {
                        GameObject.Find("me").GetComponent<Me>().touchEnabled = false;
                        SetupNextDialog(keycode - (int)KeyCode.Alpha0);
                        break;
                    }
                }
            }
        }
    }

    public void Activate()
    {
        interactionCanvas.SetActive(true);
    }

    public void DeActivate()
    {
        interactionCanvas.SetActive(false);
    }

    public void SetText(string newText)
    {
        interactionDialogBox.transform.Find("DialogText").GetComponent<TextMeshProUGUI>().text = newText;
    }
    
    public void SetCharacter(GameObject newCharacter)
    {
        interactionImage.GetComponent<Image>().sprite = newCharacter.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetCharacter(Sprite newCharacterSprite)
    {
        interactionImage.GetComponent<Image>().sprite = newCharacterSprite;
    }

    private Sprite GetCharacter()
    {
        return interactionImage.GetComponent<Image>().sprite;
    }

    public void SetInstructions(string newInstructions)
    {
        interactionInstructions.GetComponent<TextMeshProUGUI>().text = newInstructions;
    }

    public void ShowSimpleDialog(string newText, string newInstructions, Sprite newCharacterSprite, CompletionCallback newCompletionCallback)
    {
        SetText(newText);
        SetInstructions(newInstructions);
        SetCharacter(newCharacterSprite);
        completionCallback = newCompletionCallback;
        acceptableKeypresses.Clear();
        simpleDialog = true;
        Activate();
        allowInteraction = true;
    }

    public void CreateDialogLogic(string jsonString, CompletionCallback newCompletionCallback)
    {
        simpleDialog = false;
        completionCallback = newCompletionCallback;
        dialogLogic = JsonUtility.FromJson<DialogLogic>(jsonString);
        int currentDialogIndex = dialogLogic.dialogs[0].selection;
        currentDialog = dialogLogic.dialogs[currentDialogIndex - 1];
        defaultCharacter = GetCharacter();
        surveyDialogsAlreadyShown.Clear();
        currentDialogType = DialogTypes.noDialog;
        SetupNextDialog(-1);
    }

    public void SetupNextDialog(int userInput)
    {
        allowInteraction = false;
        surveyDialog.DeActivate();
        string dialogText = "";
        acceptableKeypresses.Clear();

        if ((newSong != null) && newSong.isPlaying)
        {
            newSong.Stop();
        }
        if (currentDialogType == DialogTypes.responseDialog)
        {
            //dialogText += "<color=#680606>You: <i>";
            dialogText += "<color=#a11212>You: <i>";
            dialogText += currentDialog.choices[userInput - 1].text + "</i></color>\n\n";
            SetCharacter(defaultCharacter);
        }

        if ((currentDialogType == DialogTypes.npcDialog) && (currentDialog.choices.Count > 0))
        {
            int i = 0;
            SetCharacter(meFrontSprite);
            foreach (DialogChoice choice in currentDialog.choices)
            {
                i++;
                dialogText += "<i>\n  " + i + ") " + choice.text + "</i>";
                acceptableKeypresses.Add((KeyCode)(i + (int)KeyCode.Alpha0));
            }
            SetInstructions("Press a number key to select a response.\nPress 'X' to exit.");
            currentDialogType = DialogTypes.responseDialog;
        }
        else if (userInput >= 0)
        {
            int newDialogIndex;

            if (currentDialogType == DialogTypes.responseDialog)
            {
                newDialogIndex = currentDialog.choices[userInput - 1].action;
            }
            else
            {
                newDialogIndex = currentDialog.action;
            }
            if (newDialogIndex <= 0)
            {
                AllDone(Mathf.Abs(newDialogIndex));
                return;
            }
            else
            {
                currentDialog = dialogLogic.dialogs.Find(x => (x.selection == newDialogIndex));
            }
            SetInstructions("Press any key to continue.\nPress 'X' to exit.");
            currentDialogType = DialogTypes.npcDialog;
        }
        else
        {
            SetInstructions("Press any key to continue.\nPress 'X' to exit.");
            currentDialogType = DialogTypes.npcDialog;
        }

        if (currentDialogType == DialogTypes.npcDialog)
        {
            if (currentDialog.text.StartsWith("RevealResults"))
            {
                int index = int.Parse(currentDialog.text[13..15]);
                if (!surveyDialogsAlreadyShown.Exists(x => (x == index)))
                {
                    surveyDialogsAlreadyShown.Add(index);
                    string commentary = currentDialog.text[16..];
                    surveyDialog.Activate();
                    surveyDialog.SetQAndA(index, commentary);
                }
                else
                {
                    skipDialog = true;
                }
                currentDialogType = DialogTypes.surveyDialog;
            }
            else if (currentDialog.text.StartsWith("NewImage"))
            {
                int spriteIndex = int.Parse(currentDialog.text[8..10]);
                Sprite newSprite = dialogSpriteList[spriteIndex];
                if (newSprite != null)
                {
                    SetCharacter(newSprite);
                    defaultCharacter = newSprite;
                }
                dialogText += currentDialog.text[10..] + "\n";
            }
            else if (currentDialog.text.StartsWith("NewSong"))
            {
                string songSuffix = currentDialog.text[7..9];
                newSong = GameObject.Find("song" + songSuffix).GetComponent<AudioSource>();
                if (newSong != null)
                {
                    newSong.Play();
                }
                dialogText += currentDialog.text[10..] + "\n";
            }
            else if (currentDialog.text.StartsWith("StopSong"))
            {
                newSong.Stop();
                dialogText += currentDialog.text[9..] + "\n";
            }
            else
            {
                dialogText += currentDialog.text + "\n";
            }
        }

        SetText(dialogText);
        SetupChoiceButtons();
        allowInteraction = true;
    }

    private void AllDone(int responseValue)
    {
        allowInteraction = false;
        DeActivate();
        surveyDialog.DeActivate();
        if ((newSong!=null) && (newSong.isPlaying)) newSong.Stop();
        StartCoroutine(WaitForNoInput(responseValue));
    }

    IEnumerator WaitForNoInput(int responseValue)
    {
        while (Input.anyKey || (Input.touchCount > 0) )
        {
            yield return new WaitForSeconds(0.1f);
        }
        completionCallback(responseValue);
    }

    void SetupChoiceButtons()
    {
        touchString = "";
        foreach (GameObject button in choiceButtons)
        {
            button.SetActive(false);
        }

        if (GameObject.Find("me").GetComponent<Me>().touchEnabled)
        {
            if (acceptableKeypresses.Count == 0)
            {
                choiceButtons[^2].SetActive(true);
            }

            for (int i = 0; i < acceptableKeypresses.Count; i++)
            {
                choiceButtons[i].SetActive(true);
            }

            choiceButtons[choiceButtons.Count - 1].SetActive(true);
        }
    }

    public void HandleButton(TextMeshProUGUI buttonText)
    {
        Debug.Log("Button " + buttonText.text);
        touchString = buttonText.text;
    }
}
