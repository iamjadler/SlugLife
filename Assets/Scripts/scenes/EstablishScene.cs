using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene : MonoBehaviour
{
    protected States states;

    // Start is called before the first frame update
    public virtual void Start()
    {
        states = GameObject.Find("States").GetComponent<States>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
