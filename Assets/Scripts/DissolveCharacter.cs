using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveCharacter : MonoBehaviour
{
    public ParticleSystem ps;
    public List<Sprite> dissolvingSprites;
    public List<Color> particleDissolveColors;
    public List<float> startY;
    public float initialDelay;
    public float framePeriodSeconds;
    public float vibrationAmplitude;
    private bool inDissolvingMode;
    private Vector3 startPosition;
    public delegate void CompletionCallback();
    CompletionCallback completionCallback;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    StartCoroutine(DissolveMe());
        //}
        if (inDissolvingMode)
        {
            transform.position = startPosition + new Vector3(1, 0, 0) * Random.Range(-vibrationAmplitude, vibrationAmplitude);
        }
    }

    public void StartDissolve()
    {
        // completionCallback = callback;
        StartCoroutine(DissolveMe());
    }

    IEnumerator DissolveMe()
    {
        GetComponent<SpriteRenderer>().sprite = dissolvingSprites[0];
        inDissolvingMode = true;
        yield return new WaitForSeconds(initialDelay);
        // ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.transform.position = transform.position;
        ParticleSystem.ShapeModule shape = ps.shape;
        ParticleSystem.MainModule main = ps.main;
        shape.position = new Vector2(0, startY[0]);
        main.startColor = particleDissolveColors[0];
        ps.Play();
        for (int i = 0; i < dissolvingSprites.Count; i++)
        {
            int substeps = 20;
            float yStep = (startY[i] - shape.position.y)/substeps;
            float timeStep = framePeriodSeconds / substeps;

            for (int k = 0; k < substeps; k++)
            {
                yield return new WaitForSeconds(timeStep);
                shape.position += new Vector3(0, yStep);
                Debug.Log("=" + shape.position.y+"  "+yStep);
            }
            Debug.Log("+" + shape.position.y + "  " + yStep);
            //shape.position = new Vector2(0, startY[i]);
            main.startColor = particleDissolveColors[i];
            GetComponent<SpriteRenderer>().sprite = dissolvingSprites[i];
        }
        yield return new WaitForSeconds(framePeriodSeconds);
        ps.Stop();
        yield return new WaitForSeconds(0f);
        // GetComponent<SpriteRenderer>().sprite = dissolvingSprites[0];
        inDissolvingMode = false;
        transform.position = startPosition;
        // completionCallback();
    }

}
