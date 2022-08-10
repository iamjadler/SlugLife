using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishScene0107 : EstablishScene
{
    public GameObject treeTrunk;
    public GameObject camera;
    public float trunkSpeed;
    public float cameraSpeed;
    public float trunkTranslationLimit;
    public GameObject exitTransporter;
    private float x_max, x_min, y_max, y_min, climbDownLimit;
    private GameObject me;
    private Sprite meSprite;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        states.SetState(AvailableStates.treeClimbed, "1");
        me = GameObject.Find("me");
        meSprite = me.GetComponent<SpriteRenderer>().sprite;
        me.SetActive(false);
        x_min = treeTrunk.transform.position.x - trunkTranslationLimit;
        y_min = treeTrunk.transform.position.y - trunkTranslationLimit;
        x_max = treeTrunk.transform.position.x + trunkTranslationLimit;
        y_max = treeTrunk.transform.position.y + trunkTranslationLimit;
        climbDownLimit = y_max - trunkTranslationLimit * 0.1f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 pos = treeTrunk.transform.position;
            pos.x += trunkSpeed;
            if (pos.x < x_max)
            {
                treeTrunk.transform.position = pos;
                pos = camera.transform.position;
                pos.x -= cameraSpeed;
                camera.transform.position = pos;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 pos = treeTrunk.transform.position;
            pos.x -= trunkSpeed;
            if (pos.x > x_min)
            {
                treeTrunk.transform.position = pos;
                pos = camera.transform.position;
                pos.x += cameraSpeed;
                camera.transform.position = pos;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 pos = treeTrunk.transform.position;
            pos.y += trunkSpeed;
            if (pos.y < y_max)
            {
                treeTrunk.transform.position = pos;
                pos = camera.transform.position;
                pos.y -= cameraSpeed;
                camera.transform.position = pos;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 pos = treeTrunk.transform.position;
            pos.y -= trunkSpeed;
            if (pos.y > y_min)
            {
                treeTrunk.transform.position = pos;
                pos = camera.transform.position;
                pos.y += cameraSpeed;
                camera.transform.position = pos;
            }
        }
        if (treeTrunk.transform.position.y >= climbDownLimit )
        {
            ClimbDown();
        }
    }

    void ClimbDown()
    {
        me.SetActive(true);
        me.transform.position = exitTransporter.transform.position;
        me.GetComponent<SpriteRenderer>().sprite = meSprite;
    }
}
