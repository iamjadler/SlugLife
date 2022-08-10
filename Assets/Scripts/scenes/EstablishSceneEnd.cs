using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishSceneEnd : EstablishScene
{
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        GameObject obj = GameObject.Find("InventoryContainer").transform.Find("Canvas").gameObject;
        obj.SetActive(false);
    }
}
