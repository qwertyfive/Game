using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping;
    public Vector3 offset;

    public bool isBallUp = false;
    public bool isBallLeft = false;
    public bool isBallRight = true;
    public bool isBallDown = false;

    private Transform Ball;
    private int lastX;
    private int lastZ;

    private int lastDirectionX = 1;
    private int lastDirectionY = 0;

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball").transform;

        lastX = Mathf.RoundToInt(Ball.position.x);
        lastZ = Mathf.RoundToInt(Ball.position.z);
        
        transform.position = new Vector3(Ball.position.x + offset.x, transform.position.y, Ball.position.z - offset.z);
    }

    private void FixedUpdate()
    {
        if(Ball)
        {
            int currentX = Mathf.RoundToInt(Ball.position.x);
            int currentZ = Mathf.RoundToInt(Ball.position.z);

            CheckDirection(currentX, currentZ);
            lastX = currentX;
            lastZ = currentZ;

            Vector3 target = Ball.position;

            target.y = transform.position.y;
            target.z -= offset.z;


            Vector3 currentPosition = target;
            transform.position = currentPosition;
        }
    }
    private void CheckDirection(int currentX, int currentZ)
    {
        if (currentX > lastX)
        {
            isBallRight = true;
            isBallLeft = false;
            lastDirectionX = 1;
        } else
        if(currentX < lastX)
        {
            isBallRight = false;
            isBallLeft = true;
            lastDirectionX = 2;
        } else
        {
            isBallLeft = false;
            isBallRight = false;
            if(lastDirectionX == 1) isBallRight = true;
            if (lastDirectionX == 2) isBallLeft = true;
            if (isBallUp || isBallDown) lastDirectionX = 0;
                    
        }

        if (currentZ > lastZ)
        {
            isBallUp = true;
            isBallDown = false;
            lastDirectionY = 1;
        }
        else
        if (currentZ < lastZ)
        {
            isBallUp = false;
            isBallDown = true;
            lastDirectionY = 2;
        }
        else
        {
            isBallUp = false;
            isBallDown = false;
            if (lastDirectionY == 1) isBallUp = true;
            if (lastDirectionY == 2) isBallDown = true;
            if (isBallLeft || isBallRight) lastDirectionY = 0;
        }
    }
}

