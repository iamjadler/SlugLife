using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hints : MonoBehaviour
{
    private bool waitForKey = false;
    public enum VisibilityOptions
    {
        notVisible,
        visible,
        obsolete
    };
    public delegate VisibilityOptions VisibilityFunction(States currentStates);
    public class HintRecord
    {
        public VisibilityFunction visibilityFunction;
        public string hintText;
        public HintRecord(VisibilityFunction newV, string newHintText) { visibilityFunction = newV; hintText = newHintText; }
    }

    private Dictionary<AvailableStates, HintRecord> hints = new()
    {
        { AvailableStates.meHasSlug, new(BringBanana, "You have found Slug who is willing to help you understand human interactions. Your pink-haired friend wants you to bring him the banana." ) },
        { AvailableStates.slugDiscussedEndGame, new(RetrieveId42, "You need to retrieve your ID card to return to your room and learn to communicate with your roommate.") },
        { AvailableStates.chubbsCookieQuestStarted, new(CookieQuest, "You need to find cafeteria cookies for someone.") },
        { AvailableStates.acquiredSandwich, new(CookieQuest, "Find someone to trade cookies for your sandwich.") },
        { AvailableStates.acquiredCookies, new(SupplyCookies, "You acquired cookies. Bring them to someone who was asking for them.") },
        { AvailableStates.learnedAboutPeerPressure, new(MoveFriends, "Slug has taught you that you could convince the kid blocking the bridge to move if her friends move first.") },
        { AvailableStates.pencilQuestStarted, new(InquireAboutPencil, "You need to retrieve a pencil for someone. Talk to people to find out who has it.") },
        { AvailableStates.acquiredId41, new(RetrievePencil, "You need to retrieve the pencil from her roommate in room 41 and return it to her.") },
        { AvailableStates.understoodDoubt, new(BlueFollowsOthers, "You learned that the girl in all-blue will follow others.") },
        { AvailableStates.doubtMoved, new(BridgeCanOpen, "You might be able to get the girl to allow you to cross the bridge now that her friends have all moved.") },
        { AvailableStates.bridgeTrollMoved, new(ClimbTree, "Now that you can cross the bridge, You should also be able to spot the mushroom from a spot high above.") },
        { AvailableStates.treeClimbed, new(RetrieveMushroom, "From the view at the top of the tree, you should be able to navigate to the mushroom using recognizable landmarks as guides.") },
        { AvailableStates.id42CardRetrievedFromBeach, new(CommunicateWithRoommate, "Now that you retrieved your ID 42 card, you can try to communicate with your roommate with the help of Slug.") }
    };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (waitForKey)
        {
            if (Input.anyKeyDown)
            {
                waitForKey = false;
                DeActivate();
            }
        }
    }
    private void Activate()
    {
        transform.Find("Canvas").gameObject.SetActive(true);
    }
    private void DeActivate()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    private void SetText(string newText)
    {
        transform.Find("Canvas/Text").GetComponent<TextMeshProUGUI>().text = newText;
    }

    public void DisplayAllActiveHints(States currentStates)
    {
        string hintText = "<size=150%>Hints:<size=100%>\n";

        foreach (AvailableStates stateKey in hints.Keys)
        {
            if (currentStates.DoesStateExist(stateKey))
            {
                if (hints[stateKey].visibilityFunction(currentStates) == VisibilityOptions.visible)
                {
                    hintText += "___ " + hints[(AvailableStates)stateKey].hintText + "\n";
                }
                else if (hints[stateKey].visibilityFunction(currentStates) == VisibilityOptions.obsolete)
                {
                    hintText += "_X_ " + "<s>" + hints[(AvailableStates)stateKey].hintText + "</s>\n";
                }
            }
        }
        SetText(hintText);
        Activate();
        waitForKey = true;
    }

    public static VisibilityOptions BringBanana(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.talkedToBinky2ndTime)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }

    public static VisibilityOptions RetrieveId42(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.id42CardRetrievedFromBeach)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions CookieQuest(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.acquiredCookies)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions SupplyCookies(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.chubbsMoved)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions MoveFriends(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.doubtMoved)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions InquireAboutPencil(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.acquiredId41)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions RetrievePencil(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.angieMoved)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions BlueFollowsOthers(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.doubtMoved)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions BridgeCanOpen(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.bridgeTrollMoved)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions ClimbTree(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.treeClimbed)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions RetrieveMushroom(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.retrievedMushroom)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
    public static VisibilityOptions CommunicateWithRoommate(States currentStates)
    {
        if (currentStates.DoesStateExist(AvailableStates.missionCompleted)) return VisibilityOptions.obsolete;
        else return VisibilityOptions.visible;
    }
}
