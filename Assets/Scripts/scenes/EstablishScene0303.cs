using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0303 : EstablishScene
{
    public GameObject transporterToEnter42;
    public GameObject transporterToEnter41;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        if (!GameObject.Find("InventoryContainer").GetComponent<Inventory>().DoesExist("id42"))
        {
            transporterToEnter42.SetActive(false);
        }
        if (!GameObject.Find("InventoryContainer").GetComponent<Inventory>().DoesExist("id41"))
        {
            transporterToEnter41.SetActive(false);
        }
    }
}
