using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloopers : MonoBehaviour
{
    public Sprite jason, mika, meFront;
    public GameObject slugMessy, meWithFace, cancellation, emoteDancing;
    public GameObject bloopersBackground;
    public AudioSource beep, happyMusic;

    public delegate void CompletionCallback();
    CompletionCallback completionCallback;
    CompletionCallback afterKeypressCode;

    InteractionDialog dialog;
    private GameObject me;
    delegate void BlooperCode();
    int currentIndex = 0;
    bool waitForKeypress = false;

    List<BlooperCode> bloopers = new();

    // Start is called before the first frame update
    void Start()
    {
        me = GameObject.Find("me");

        dialog = GameObject.Find("InteractionContainer").GetComponent<InteractionDialog>();
        bloopers.Add(StartMessySlugDialog);
        bloopers.Add(StartCancelledDialog);
        bloopers.Add(StartEmoteDancingDialog);
        bloopers.Add(StartMeWithFaceDialog);

        bloopers.Add(AllDone);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForKeypress)
        {
            if (Input.anyKeyDown)
            {
                waitForKeypress = false;
                afterKeypressCode();
            }
        }
    }

    public void StartBloopers(CompletionCallback newCompletionCallback)
    {
        me.SetActive(false);

        bloopersBackground.SetActive(true);
        completionCallback = newCompletionCallback;
        currentIndex = 0;
        bloopers[currentIndex]();
    }

    private void StartNextBlooper(int response=0)
    {
        currentIndex++;
        bloopers[currentIndex]();
    }

    void StartMessySlugDialog()
    {
        string instructions = "Press any key to continue.";
        string text = "Slug, can't you just eat the mushroom without climbing all over it?";
        dialog.ShowSimpleDialog(text, instructions, jason, ShowMessySlug);
    }

    void ShowMessySlug(int response=0)
    {
        Debug.Log("ShowMessySlug");
        slugMessy.SetActive(true);
        WaitForKeypress(DoneMessySlug);
    }

    void DoneMessySlug()
    {
        Debug.Log("DoneMessySlug");
        slugMessy.SetActive(false);
        beep.Play();
        StartNextBlooper();
    }

    void StartCancelledDialog()
    {
        Debug.Log("StartCancelledDialog");
        string instructions = "Press any key to continue.";
        string text = "Dude. You can't say that. You're gonna get cancelled.";
        dialog.ShowSimpleDialog(text, instructions, mika, ShowCancelled);
    }

    void ShowCancelled(int response = 0)
    {
        Debug.Log("ShowCancelled");
        cancellation.SetActive(true);
        cancellation.GetComponent<DissolveCharacter>().StartDissolve();
        WaitForKeypress(DoneCancelled);
    }

    void DoneCancelled()
    {
        Debug.Log("DoneCancelled");
        cancellation.SetActive(false);
        string instructions = "Press any key to continue.";
        string text = "Wow! I won't say THAT ever again.";
        dialog.ShowSimpleDialog(text, instructions, meFront, DoneCancelledDialog);
    }

    void DoneCancelledDialog(int response)
    {
        beep.Play();
        StartNextBlooper();
    }

    void StartEmoteDancingDialog()
    {
        StartCoroutine(ShowEmoteDancing());
    }

    IEnumerator ShowEmoteDancing()
    {
        yield return new WaitForSeconds(1f);
        happyMusic.Play();
        emoteDancing.SetActive(true);
        yield return new WaitForSeconds(5);
        WaitForKeypress(DoneDancing);
    }

    void DoneDancing()
    {
        happyMusic.Stop();
        beep.Play();
        emoteDancing.SetActive(false);
        StartNextBlooper();
    }

    void StartMeWithFaceDialog()
    {
        string instructions = "Press any key to continue.";
        string text = "Oh. That's what your face looks like.";
        dialog.ShowSimpleDialog(text, instructions, mika, ShowMeWithFace);
    }

    void ShowMeWithFace(int response = 0)
    {
        meWithFace.SetActive(true);
        WaitForKeypress(DoneMeWithFace);
    }

    void DoneMeWithFace()
    {
        beep.Play();
        meWithFace.SetActive(false);
        StartNextBlooper();
    }

    void AllDone()
    {
        bloopersBackground.SetActive(false);
        me.SetActive(true);
        completionCallback();
    }

    void WaitForKeypress(CompletionCallback callback)
    {
        StartCoroutine(WaitForKeypressCoroutine(callback));
    }

    IEnumerator WaitForKeypressCoroutine(CompletionCallback afterKeypress)
    {
        yield return new WaitForSeconds(0f);
        afterKeypressCode = ()=> { StartCoroutine(WaitAFrameToCall(afterKeypress)); };
        waitForKeypress = true;
    }

    IEnumerator WaitAFrameToCall(CompletionCallback afterKeypress)
    {
        yield return new WaitForSeconds(0f);
        afterKeypress();
    }

    IEnumerator WaitForNoKeypress()
    {
        while (Input.anyKey)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
