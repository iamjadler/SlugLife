using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonBox : MonoBehaviour
{
    private bool imActive = false;
    public delegate void Callback();
    Callback completionCallback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (imActive)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(HideLessonBox());
            }
        }
    }

    public void ShowLessonBox(Callback callback)
    {
        completionCallback = callback;
        transform.Find("lessonBox").gameObject.SetActive(true);
        imActive = true;
    }

    IEnumerator HideLessonBox()
    {
        imActive = false;
        transform.Find("lessonBox").gameObject.SetActive(false);
        yield return new WaitForSeconds(0f);
        completionCallback();
    }
}
