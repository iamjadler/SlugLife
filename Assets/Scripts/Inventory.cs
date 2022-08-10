using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Inventory : MonoBehaviour
{
    public GameObject inventoryBox;
    public Canvas canvas;
    private int count = 0;
    private List<GameObject> objectList = new List<GameObject>();
    private int MAX_COUNT = 8;
    private bool inDropMode = false; // jra remove this
    private States states;
    private GameObject me;

    // Animation variables
    private float animationCounter = 1f;
    private Vector3 startPosition, endPosition, startScale, maxScale;
    private string originalSortingLayer;
    private GameObject objectBeingAnimated;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        states = GameObject.Find("States").GetComponent<States>();
        me = GameObject.Find("me");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inDropMode = true;
        }    
        if (inDropMode)
        {
            if (Input.anyKeyDown)
            {
                for (int i=0; i<count; i++)
                {
                    if (Input.GetKeyDown((KeyCode)(i+(int)KeyCode.Alpha0)))
                    {
                        GameObject objectFromInventory = Remove(i);
                        objectFromInventory.GetComponent<Collectable>().Drop(me.transform.position);
                    }
                }
                inDropMode = false;
            }
        }
    }

    public void Activate()
    {
        canvas.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        canvas.gameObject.SetActive(false);
    }

    public void Add(GameObject item)
    {
        if (count < MAX_COUNT)
        {
            count++;
            GameObject itemHolder = inventoryBox.transform.Find("Item" + count).gameObject;
            itemHolder.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            objectList.Add(item);
        }
        else
        {
            Debug.LogWarning("Inventory Full!");
        }
    }

    public bool DoesExist(string itemName)
    {
        return objectList.Exists(x=>(x.name==itemName));
    }

    public int Find(GameObject item)
    {
        int index = objectList.IndexOf(item);
        return index;
    }

    public GameObject Remove(GameObject item)
    {
        int index = Find(item);
        if (index >= 0)
        {
            return Remove(index);
        }
        else
        {
            Debug.LogWarning("Item to be removed from Inventory is not found");
            return null;
        }
    }

    public GameObject Remove(int index)
    {
        GameObject removedObject;
        if ((index < count) && (index >= 0))
        {
            removedObject = objectList[index];
            objectList.RemoveAt(index);
            GameObject itemHolder = inventoryBox.transform.Find("Item" + (index+1)).gameObject;
            for (int i=index+1; i<count; i++)
            {
                Image destImage   = inventoryBox.transform.Find("Item" + i).gameObject.GetComponent<Image>();
                Image sourceImage = inventoryBox.transform.Find("Item" + (i+1)).gameObject.GetComponent<Image>();
                destImage.sprite = sourceImage.sprite;
            }
            inventoryBox.transform.Find("Item" + count).gameObject.GetComponent<Image>().sprite = null;
            count--;
        }
        else
        {
            Debug.LogWarning("Item index to be removed from Inventory is not found");
            removedObject = null;
        }
        return removedObject;
    }
}
