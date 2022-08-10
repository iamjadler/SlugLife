using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnimation : MonoBehaviour
{
    public GameObject mushroom1;
    public GameObject mushroom2;
    public float periodInSeconds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            mushroom1.SetActive(false);
            mushroom2.SetActive(false);
            yield return new WaitForSeconds(periodInSeconds);
            mushroom1.SetActive(false);
            mushroom2.SetActive(true);
            yield return new WaitForSeconds(periodInSeconds);
            mushroom1.SetActive(true);
            mushroom2.SetActive(false);
            yield return new WaitForSeconds(periodInSeconds);
            mushroom1.SetActive(false);
            mushroom2.SetActive(true);
            yield return new WaitForSeconds(periodInSeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
