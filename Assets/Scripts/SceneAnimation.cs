using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimation : MonoBehaviour
{
    public float period;
    private int frameCount=0;
    List<GameObject> frame = new();
    float lastTime=0f;
    int currentFrame=0;

    // Start is called before the first frame update
    void Start()
    {
        frameCount = transform.childCount;
        currentFrame = frameCount - 1;
        for (int i=0; i< frameCount; i++)
        {
            frame.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float now = Time.time;
        if (now - lastTime > period)
        {
            lastTime = now;
            frame[currentFrame].SetActive(false);
            currentFrame++;
            if (currentFrame >= frameCount) { currentFrame = 0; }
            frame[currentFrame].SetActive(true);
        }
    }
}
