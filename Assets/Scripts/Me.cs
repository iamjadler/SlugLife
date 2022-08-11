using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Me : MonoBehaviour
{
    public float moveSpeed;
    private bool inCollision = false;
    private bool inDialog = false;
    private bool touchMode = false;
    private string currentScene;
    private Animator walkingAnimator;
    private Rigidbody2D rb2d;
    private States states;
    private bool ignoreNextCollision = false;
    private const float bound_buffer = 10;
    private Vector2 lower_bounds;
    private Vector2 upper_bounds;
    private bool performingSceneTransition = false;
    public bool touchEnabled = false;
    private float scaleFactor = 1.0f;
    private Vector2 normalScale;
    private float normalMoveSpeed;
    private GameObject debugWindow;
    private GameObject slug;
    private GameObject ignoreWho;
    private AudioSource walkingAudio;
    public AudioClip indoorAudioClip;
    public AudioClip outdoorAudioClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;

        walkingAnimator = gameObject.GetComponent<Animator>();

        rb2d = GetComponent<Rigidbody2D>();

        states = GameObject.Find("States").GetComponent<States>();
        debugWindow = GameObject.Find("DebugWindow");
        lower_bounds = new Vector2(-Globals.scene_width / 2 + bound_buffer, -Globals.scene_height / 2 + bound_buffer);
        upper_bounds = new Vector2( Globals.scene_width / 2 - bound_buffer,  Globals.scene_height / 2 - bound_buffer);

        slug = GameObject.Find("slug");
        slug.SetActive(false);

        walkingAudio = transform.Find("WalkingAudio").gameObject.GetComponent<AudioSource>();

        normalScale = transform.localScale;
        normalMoveSpeed = moveSpeed;
    }

    public GameObject GetSlug()
    {
        return slug;
    }

    enum TouchCommands
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    TouchCommands touchCommand = TouchCommands.None;
    float neutralDistance = 20f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inDialog)
        {
            touchCommand = TouchCommands.None;
            if ((Input.touchCount > 0) || (Input.GetButton("Fire1")))
            {
                Vector2 relativeTouch;
                touchEnabled = true;
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Debug.Log("" + touch.position + " " + (Vector2)transform.position);
                    relativeTouch = touch.position - new Vector2(Globals.scene_width, Globals.scene_height) / 2 - (Vector2)transform.position;
                }
                else
                {
                    relativeTouch = Input.mousePosition - new Vector3(Globals.scene_width, Globals.scene_height)/2 - transform.position;
                }

                if ( (Mathf.Abs(relativeTouch.x) > Mathf.Abs(relativeTouch.y)) && (Mathf.Abs(relativeTouch.x) > neutralDistance) )
                {
                    if (relativeTouch.x <= 0) { touchCommand = TouchCommands.Left; }
                    else { touchCommand = TouchCommands.Right; }
                }
                else if (Mathf.Abs(relativeTouch.y) > neutralDistance )
                {
                    if (relativeTouch.y <= 0) { touchCommand = TouchCommands.Down; }
                    else { touchCommand = TouchCommands.Up; }
                }
            }

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKey(KeyCode.R))
                {
                    if (moveSpeed > normalMoveSpeed) { moveSpeed = normalMoveSpeed; }
                    else { moveSpeed *= 3; }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    debugWindow.transform.Find("Canvas").gameObject.SetActive(!debugWindow.transform.Find("Canvas").gameObject.activeSelf);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow) || (touchCommand == TouchCommands.Down))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (rb2d.velocity == new Vector2(0, -moveSpeed)) walkingAnimator.SetTrigger("startWalkingForward");
                else rb2d.velocity = new Vector2(0, -moveSpeed);
            }
            else if (Input.GetKey(KeyCode.UpArrow) || (touchCommand == TouchCommands.Up))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (rb2d.velocity == new Vector2(0, moveSpeed)) walkingAnimator.SetTrigger("startWalkingBackward");
                else rb2d.velocity = new Vector2(0, moveSpeed);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || (touchCommand == TouchCommands.Left))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (rb2d.velocity == new Vector2(-moveSpeed, 0)) walkingAnimator.SetTrigger("startWalkingLeft");
                else rb2d.velocity = new Vector2(-moveSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || (touchCommand == TouchCommands.Right))
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (rb2d.velocity == new Vector2(moveSpeed, 0)) walkingAnimator.SetTrigger("startWalkingRight");
                else rb2d.velocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                walkingAnimator.SetTrigger("stopWalking");
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        if (rb2d.velocity.Equals(new Vector2(0, 0)))
        {
            if (walkingAudio.isPlaying) walkingAudio.Stop();
        }
        else
        {
            if (!walkingAudio.isPlaying) walkingAudio.Play();
        }

        float x = transform.position.x;
        float y = transform.position.y;

        if (x < lower_bounds.x) { transform.position = new Vector2(lower_bounds.x, y); }
        if (x > upper_bounds.x) { transform.position = new Vector2(upper_bounds.x, y); }
        if (y < lower_bounds.y) { transform.position = new Vector2(x, lower_bounds.y); }
        if (y > upper_bounds.y) { transform.position = new Vector2(x, upper_bounds.y); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string whoDidIHit = collision.gameObject.name;
        string tag = collision.gameObject.tag;

        if ((ignoreNextCollision) && (collision.gameObject == ignoreWho))
        {
            return;
        }

        if (performingSceneTransition)
        {
            return;
        }

        if (tag == "furniture")
        {
            BackOffCollision(collision);
        }
        else if (tag == "character")
        {
            if (scaleFactor >= 1.0f)
            {
                inDialog = true;
                BackOffCollision(collision);
                collision.gameObject.GetComponent<Character>().StartInteraction(InteractionCompleteCallback);
            }
        }
        else if (tag == "collectable")
        {
            //Debug.Log("Collectable hit");
            if ((collision.gameObject.name == "id42") && (states.GetState(AvailableStates.id42HasBeenPickedUp) != "1"))
            {
                states.SetState(AvailableStates.id42HasBeenPickedUp, "1");
                collision.gameObject.GetComponent<Collectable>().PickUp();
                // GameObject.Find("lessonBoxContainer").GetComponent<LessonBox>().ShowLessonBox(InventoryLessonCompleted);
            }
            else if ((collision.gameObject.name == "id42") && (states.GetState(AvailableStates.id42CardRetrievedFromBeach) != "1"))
            {
                states.SetState(AvailableStates.id42CardRetrievedFromBeach, "1");
                collision.gameObject.GetComponent<Collectable>().PickUp();
            }
            else
            {
                collision.gameObject.GetComponent<Collectable>().PickUp();
            }
        }
        else if (whoDidIHit.StartsWith("transporterTo"))
        {
            if (collision.gameObject.GetComponent<TransporterTo>().IsEnabled())
            {
                PrepareToLeaveScene();
                string newScene = "Scene" + whoDidIHit[13..];
                if (states.GetState(AvailableStates.missionCompleted) == "1")
                {
                    newScene = "SceneEnd";
                }
                Debug.Log("Loading scene " + newScene);
                SceneManager.LoadScene(newScene);
            }
        }
        else if (tag == "bloopers")
        {
            inDialog = true;
            BackOffCollision(collision);
            walkingAnimator.SetTrigger("stopWalking");
            GameObject.Find("Bloopers").GetComponent<Bloopers>().StartBloopers(DoneWithBloopers);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if ((ignoreNextCollision) && (collision.gameObject == ignoreWho))
        {
            return;
        }

        if (performingSceneTransition)
        {
            return;
        }

        if (tag == "furniture")
        {
            BackOffCollision(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ignoreNextCollision = false;
        inCollision = false;
    }

    private void BackOffCollision(Collision2D collision)
    {
        inCollision = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        ColliderDistance2D dist2D = GetComponent<Collider2D>().Distance(collision.collider);
        // backoff from collision by the amount of overlap + 10% in the 'normal' direction
        transform.position += (Vector3)(dist2D.normal * dist2D.distance * 2f);
    }

    public void IgnoreNextCollision(GameObject newIgnoreWho)
    {
        ignoreNextCollision = true;
        ignoreWho = newIgnoreWho;
    }

    private void InteractionCompleteCallback(int responseValue)
    {
        inDialog = false;
    }

    private void DoneWithBloopers()
    {
        inDialog = false;
    }

    public void PrepareForFirstScene()
    {
        performingSceneTransition = true;
    }

    void PrepareToLeaveScene()
    {
        performingSceneTransition = true;
        foreach (KeyValuePair<string, TrackableObjectState> objState in states.GetAllObjectsInScene(currentScene))
        {
            objState.Value.obj.SetActive(false);
        }
        slug.SetActive(false);
    }

    void InventoryLessonCompleted()
    {
        GameObject.Find("id42").GetComponent<Collectable>().PickUp();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        string currentSceneSuffix = currentScene[5..];
        Debug.Log("transporterFrom" + currentSceneSuffix + "  " + GameObject.Find("transporterFrom"+currentSceneSuffix));
        if (scaleFactor == 1.0f)
        {
            transform.position = GameObject.Find("transporterFrom" + currentSceneSuffix).transform.position;
        }
        else
        {
            transform.position = GameObject.Find("transporterFrom" + currentSceneSuffix + "Lower").transform.position;
        }
        if (GameObject.Find("indoors") != null)
        {
            walkingAudio.clip = indoorAudioClip;
        }
        else
        {
            walkingAudio.clip = outdoorAudioClip;
        }

        currentScene = SceneManager.GetActiveScene().name;
        foreach (KeyValuePair<string, TrackableObjectState> objState in states.GetAllObjectsInScene(currentScene))
        {
            if (objState.Value.sceneObjectType == StatesObjectType.collectableType)
            {
                GameObject obj = objState.Value.obj;
                obj.transform.position = objState.Value.position;
                obj.SetActive(true);
            }
        }
        performingSceneTransition = false;
    }

    public float GetScaleFactor()
    {
        return scaleFactor;
    }

    public void SetScaleFactor(float newScaleFactor)
    {
        scaleFactor = newScaleFactor;
        transform.localScale = normalScale * scaleFactor;
        moveSpeed = normalMoveSpeed * scaleFactor;
    }
}
