using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class Startup : MonoBehaviour
{
    public GameObject interactionPrefab;
    public GameObject surveyPrefab;
    public GameObject inventoryPrefab;
    public GameObject states;
    public GameObject debugWindow;

    public GameObject slug;
    public GameObject me;

    public string startScene;
    public TextMeshProUGUI versionText;
    private bool waitForKeypress = false;

    // Start is called before the first frame update
    void Start()
    {
        // if (GameObject.Find("me") == null)
        {
            GameObject obj = inventoryPrefab.transform.Find("Canvas").gameObject;
            obj.SetActive(false);
            obj = debugWindow.transform.Find("Canvas").gameObject;
            obj.SetActive(false);

            // me and slug start as inactive to make sure their awake/start functions aren't called until other components have started.  Make them active now.
            me.SetActive(true);
            slug.SetActive(true);

            versionText.text = "Ver "+Application.version;

            me.GetComponent<Me>().PrepareForFirstScene();

            waitForKeypress = true;
        }
    }

    void StartGame()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {

            string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            states.GetComponent<States>().AddNewScene(sceneName);
        }
        states.GetComponent<States>().AddNewScene("Inventory");
        // SceneManager.LoadScene("Scene4-4");
        SceneManager.LoadScene(startScene);

        // jra
        //for (int i = 0; i < (int)AvailableStates.veryLastState; i++)
        //{
        //    states.GetComponent<States>().SetState((AvailableStates)i, "1");
        //}

        GameObject obj = inventoryPrefab.transform.Find("Canvas").gameObject;
        obj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForKeypress && (Input.anyKeyDown))
        {
            waitForKeypress = false;
            StartGame();
        }
    }
}
