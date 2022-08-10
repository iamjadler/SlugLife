using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesObjectType
{
    collectableType,
    objectType
}

public class TrackableObjectState
{
    public GameObject obj;
    public StatesObjectType sceneObjectType;
    public string sceneName;
    public Vector2 position;
    public string state;

    public TrackableObjectState(GameObject newObj, StatesObjectType newObjectType, string newSceneName, Vector2 newPosition, string newState)
    {
        obj = newObj;
        sceneObjectType = newObjectType;
        sceneName = newSceneName;
        position = newPosition;
        state = newState;
    }
}

public class States : MonoBehaviour
{


    // a dictionary of collectables
    private Dictionary<string, TrackableObjectState> trackableObjectStates = new();
    // a dictionary of scenes, and within each scene a dictionary of objects (some are collectables others are static objects)
    private Dictionary<string, Dictionary<string, TrackableObjectState>> sceneObjectStates = new();
    private Dictionary<AvailableStates, string> stringStates = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTrackableObject(GameObject trackableObject, string newScene, Vector2 newPosition)
    {
        string oldScene = trackableObjectStates[trackableObject.name].sceneName;
        trackableObjectStates[trackableObject.name].sceneName = newScene;
        trackableObjectStates[trackableObject.name].position = newPosition;
        sceneObjectStates[oldScene].Remove(trackableObject.name);
        sceneObjectStates[newScene].Add(trackableObject.name, trackableObjectStates[trackableObject.name]);
    }

    public void AddNewTrackableObject(GameObject objectToAdd, StatesObjectType newObjectType, string sceneName, Vector2 position, string newState)
    {
        if (!trackableObjectStates.ContainsKey(objectToAdd.name))
        {
            trackableObjectStates.Add(objectToAdd.name, new TrackableObjectState(objectToAdd, newObjectType, sceneName, position, newState));
            sceneObjectStates[sceneName].Add(objectToAdd.name, trackableObjectStates[objectToAdd.name]);
        }
    }

    public void AddNewScene(string sceneName)
    {
        if (!sceneObjectStates.ContainsKey(sceneName))
        {
            sceneObjectStates.Add(sceneName, new());
        }
    }

    public bool DoesObjectExist(string trackableObjectName)
    {
        return trackableObjectStates.ContainsKey(trackableObjectName);
    }

    public GameObject GetObjectByName(string trackableObjectName)
    {
        return trackableObjectStates[trackableObjectName].obj;
    }

    public string GetObjectState(GameObject trackableObject)
    {
        return trackableObjectStates[trackableObject.name].state;
    }

    public void SetObjectState(GameObject trackableObject, string newState)
    {
        trackableObjectStates[trackableObject.name].state = newState;
    }

    public Dictionary<string, TrackableObjectState> GetAllObjectsInScene(string sceneName)
    {
        return sceneObjectStates[sceneName];
    }

    public bool DoesStateExist(AvailableStates stateName)
    {
        return stringStates.ContainsKey(stateName);
    }

    public string GetState(AvailableStates stateName)
    {
        if (DoesStateExist(stateName))
        {
            return stringStates[stateName];
        }
        else
        {
            return "NONE";
        }
    }

    public void SetState(AvailableStates stateName, string newState)
    {
        if (newState == "NONE")
        {
            Debug.LogError("State value of 'NONE' is not allowed");
        }
        Debug.Log("Set State " + stateName + " to " + newState);
        stringStates[stateName] = newState;

        float complete = (float)stringStates.Count / (float)AvailableStates.veryLastState;
        GameObject.Find("ProgressContainer").GetComponent<Progress>().SetProgress(complete);
    }


}

public enum AvailableStates
{
    id42HasBeenPickedUp,
    bananaQuestStarted,
    meHasSlug,
    talkedToBinky2ndTime,
    slugDiscussedEndGame,
    chubbsCookieQuestStarted,
    cafeteriaOpen,
    acquiredSandwich,
    acquiredCookies,
    learnedAboutPeerPressure,
    pencilQuestStarted,
    helpedAnxyRemember,
    talkedToBinkyAboutPencil,
    acquiredId41,
    acquiredPencil,
    understoodDoubt,
    angieMoved,
    chubbsMoved,
    doubtMoved,
    bridgeTrollMoved,
    id42CardRetrievedFromBeach,
    treeClimbed,
    retrievedMushroom,
    missionCompleted,
    veryLastState
}