using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    private Inventory inventory;
    protected States states;
    private float animationCounter = 1f;
    private Vector3 startPosition, endPosition, startScale, maxScale;
    string originalSortingLayer;
    public delegate void AnimationCompleteCallback();
    AnimationCompleteCallback localAnimationCompleteCallback, globalAnimationCompleteCallback;

    private void Awake()
    {
        states = GameObject.Find("States").GetComponent<States>();
        inventory = GameObject.Find("InventoryContainer").GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    virtual public void Start()
    {
        if (!states.DoesObjectExist(gameObject.name))
        {
            // this is the first time entering a scene with this object
            DontDestroyOnLoad(gameObject);
            states.AddNewTrackableObject(gameObject, StatesObjectType.collectableType, SceneManager.GetActiveScene().name, transform.position, "");
        }
        else
        {
            // we've already moved this object to persistent "scene", so destroy this duplicate in the current scene
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animationCounter < 1f)
        {
            animationCounter += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, animationCounter);
            transform.localScale = Vector3.Lerp(startScale, maxScale, 0.5f-Mathf.Abs(0.5f-animationCounter));
            if (animationCounter >= 1f)
            {
                localAnimationCompleteCallback();
            }
        }
    }

    virtual public void PickUp(bool animate = true, AnimationCompleteCallback callback = null)
    {
        if (animate)
        {
            startPosition = transform.position;
            float endx = GameObject.Find("InventoryBoxOutline").transform.position.x / Screen.width * Globals.scene_width;
            float endy = GameObject.Find("InventoryBoxOutline").transform.position.y / Screen.height * Globals.scene_height;
            endPosition = new Vector3(endx, endy, 0.0f) - new Vector3(Globals.scene_width, Globals.scene_height, 0.0f) / 2;
            Debug.Log("Inventory Pos: " + endPosition + " " + (GameObject.Find("InventoryBoxOutline").transform.localPosition));
            Debug.Log("  " + Screen.width + "x" + Screen.height);
            startScale = transform.localScale;
            maxScale = startScale * 5;
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("me").GetComponent<BoxCollider2D>(), ignore: true);
            originalSortingLayer = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";
            localAnimationCompleteCallback = PickUpAnimationCompleted;
            globalAnimationCompleteCallback = callback;
            animationCounter = 0f;
        }
        else
        {
            inventory.Add(gameObject);
            states.MoveTrackableObject(gameObject, "Inventory", new());
            gameObject.SetActive(false);
        }
    }

    void PickUpAnimationCompleted()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("me").GetComponent<BoxCollider2D>(), ignore: false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = originalSortingLayer;
        transform.localScale = startScale;
        inventory.Add(gameObject);
        states.MoveTrackableObject(gameObject, "Inventory", new());
        gameObject.SetActive(false);
        if (globalAnimationCompleteCallback!=null) { globalAnimationCompleteCallback(); };
    }

    public void Drop(Vector3 dropPosition, bool animate = true, AnimationCompleteCallback callback = null)
    {
        GameObject me = GameObject.Find("me");
        inventory.Remove(gameObject);
        gameObject.SetActive(true);
        if (animate)
        {
            me.GetComponent<Me>().IgnoreNextCollision(gameObject);
            float startx = GameObject.Find("InventoryBoxOutline").transform.position.x / Screen.width * Globals.scene_width;
            float starty = GameObject.Find("InventoryBoxOutline").transform.position.y / Screen.height * Globals.scene_height;
            startPosition = new Vector3(startx, starty, 0.0f) - new Vector3(Globals.scene_width, Globals.scene_height, 0.0f) / 2;
            // endPosition = me.transform.position;
            endPosition = dropPosition;
            startScale = gameObject.transform.localScale;
            maxScale = startScale * 5;
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), me.GetComponent<BoxCollider2D>(), ignore: true);
            originalSortingLayer = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";
            localAnimationCompleteCallback = DropAnimationCompleted;
            globalAnimationCompleteCallback = callback;
            animationCounter = 0f;
        }
        else
        {
            transform.position = dropPosition;
            states.MoveTrackableObject(gameObject, SceneManager.GetActiveScene().name, dropPosition);
        }
    }

    void DropAnimationCompleted()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("me").GetComponent<BoxCollider2D>(), ignore: false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = originalSortingLayer;
        transform.localScale = startScale;
        states.MoveTrackableObject(gameObject, SceneManager.GetActiveScene().name, transform.position);
        if (globalAnimationCompleteCallback != null) { globalAnimationCompleteCallback(); };
    }


}
