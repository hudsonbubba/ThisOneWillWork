using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlideCam : MonoBehaviour
{

    public float slideSpeed;

    public GameEvent camSlideEnd;

    public Ship playerShipSO;
    private int playerSpeed;

    private const float xDist = 2.4f;

    bool sliding = false;

    Vector3 camStartPos;
    Vector3 targetCamPos;

    // Start is called before the first frame update
    void Start()
    {
        camStartPos = transform.position;

        playerSpeed = playerShipSO.speed;
    }
    void Update()
    {
        if(sliding)
        {
            SlidingAnim();
        }

    }

    public void SlidingAnim()
    {

        float step = slideSpeed * Time.deltaTime;

        if (transform.position.x < targetCamPos.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetCamPos, step);
        }
        else if (transform.position.x == targetCamPos.x)
        {
            sliding = false;
            camSlideEnd.Raise();
            transform.position = camStartPos;
        }
        else
        {
            sliding = false;
        }

    }

    public void e_SlideStart()
    {
        playerSpeed = playerShipSO.speed;
        sliding = true;
        targetCamPos = new Vector3(transform.position.x + (playerSpeed * xDist), transform.position.y, transform.position.z);
    }
}
