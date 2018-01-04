using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newsPaperScript : MonoBehaviour {

    public int newspaperId = 1;

    private bool showText;
    //private RectTransform imageRectTransform;

    private Vector3 pointA;
    private Vector3 pointB;

    private GameObject player;

    void Start()
    {
        //pressed = false;
        showText = false;

        pointA = transform.position;
        pointB = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (showText)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            pointA = screenPos;
            pointA.x = pointA.x + 0;
            pointA.y = pointA.y + 0;
            pointB = pointA;
            pointB.x = pointB.x + 0;
            pointB.y = pointB.y + 0;

            Vector3 differenceVector = pointB - pointA;


        }
        else
        {
            //imageRectTransform.position = new Vector3(-100, -100, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            showText = true;
            player = other.gameObject;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            showText = false;
        }
    }

    

    void OnGUI()
    {
        if (showText)
        {
            GUI.Label(new Rect(pointA.x + 5f, Screen.height - pointA.y - 43f, 100f, 100f), "Click to take Newspaper");
        }
    }

    void pickedUp()
    {
        showText = false;
        Destroy(gameObject);
    }

    public void pickingUp()
    {
        player.SendMessage("takeNewspaper", newspaperId);
    }
}
