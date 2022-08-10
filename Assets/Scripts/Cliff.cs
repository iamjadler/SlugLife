using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DownCliffDirection
{
    up,
    down,
    left,
    right
}

public class Cliff : MonoBehaviour
{
    public DownCliffDirection downCliffDirection;
    private bool inCollision = false;
    private Me meObjInCollision;
    Vector3 originalScale;
    float startCliffY, halfHeight, direction;

    // Start is called before the first frame update
    void Start()
    {
        halfHeight = GetComponent<BoxCollider2D>().bounds.extents.y;
        if (downCliffDirection == DownCliffDirection.up)
        {
            startCliffY = transform.position.y - halfHeight;
            direction = 1;
        }
        else
        {
            startCliffY = transform.position.y + halfHeight;
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inCollision)
        {
            float scaleFactor = Mathf.Lerp(1.0f, 0.25f, direction*(meObjInCollision.transform.position.y - startCliffY) / (2.0f*halfHeight));
            meObjInCollision.SetScaleFactor(scaleFactor);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inCollision = true;
        meObjInCollision = collision.gameObject.GetComponent<Me>();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        inCollision = false;
        if (Mathf.Abs(meObjInCollision.GetScaleFactor()-1.0f) < 0.1f)
        {
            // we just finished going up the cliff
            meObjInCollision.SetScaleFactor(1.0f);
        }
    }
}
