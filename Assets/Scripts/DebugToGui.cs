using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugToGui : MonoBehaviour
{
    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new Queue();
    TextMeshProUGUI textBox;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        textBox = transform.Find("Canvas/Text").GetComponent<TextMeshProUGUI>();
        Debug.Log("Started up logging.");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();

        textBox.text = string.Join("\n", myLogQueue.ToArray());
    }

}
