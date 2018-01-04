using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour {

    public bool closed;
    public enum openDirection { NONE, LEFT, RIGHT, UP, DOWN };
    public openDirection opens;

    private Vector3 startPosition;
    private Vector3 closedPosition;

	void Start () {
        startPosition = transform.position;
        closedPosition = transform.position;
        if (!closed)
        {
            if (opens == openDirection.RIGHT)
            {
                closedPosition.x = closedPosition.x - 1.0f;
            }
            else if (opens == openDirection.LEFT)
            {
                closedPosition.x = closedPosition.x + 1.0f;
            }
            else if (opens == openDirection.UP)
            {
                closedPosition.y = closedPosition.y - 1.0f;
            }
            else
            {
                closedPosition.y = closedPosition.y + 1.0f;
            }
        }
        else
        {
            if (opens == openDirection.RIGHT)
            {
                startPosition.x = startPosition.x + 1.0f;
            }
            else if (opens == openDirection.LEFT)
            {
                startPosition.x = startPosition.x - 1.0f;
            }
            else if (opens == openDirection.UP)
            {
                startPosition.y = startPosition.y + 1.0f;
            }
            else
            {
                startPosition.y = startPosition.y - 1.0f;
            }
        }
    }
	
	void Update () {
        if (closed)
        {
            transform.position = closedPosition;
        }
        else
        {
            transform.position = startPosition;
        }
    }

    public void toggleDoor()
    {
        if (closed)
        {
            closed = false;
        }
        else
        {
            closed = true;
        }
    }
}
