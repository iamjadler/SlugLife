using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Progress : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetProgress(float newProgress)
    {
        progressText.text = "" + (int)(newProgress * 100 + 0.5) + "% Complete";
    }
}
