using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingTriggerScript : MonoBehaviour {

    public bool onlyOnce = false;
    private bool triggeredAlready = false;

    public bool playerTriggers = true;
    public bool humanTriggers = false;
    public bool zombieTriggers = false;

    public bool startEvent = false;
    public int eventToStart = 0;
    private bool eventPlayed = false;


    public GameObject targetLight1;
    public GameObject targetLight2;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player" && playerTriggers) 
            || (other.gameObject.tag == "human" && humanTriggers) 
            || (other.gameObject.tag == "zombie" && zombieTriggers))
        {
            if (onlyOnce && !triggeredAlready)
            {
                triggerStart();
                triggeredAlready = true;
            }
            else if (!onlyOnce)
            {
                triggerStart();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player" && playerTriggers)
            || (other.gameObject.tag == "human" && humanTriggers)
            || (other.gameObject.tag == "zombie" && zombieTriggers))
        {
            if (onlyOnce && !triggeredAlready)
            {
                triggerStop();
                triggeredAlready = true;
            }
            else if (!onlyOnce)
            {
                triggerStop();
            }
        }
    }

    void triggerStart()
    {
        if (startEvent)
        {
            GameObject player = GameObject.FindGameObjectWithTag("player");
            player.SendMessage("startEvent", eventToStart);
        }
        if (targetLight1 != null)
        {
            targetLight1.SetActive(true);
        }
        if (targetLight2 != null)
        {
            targetLight2.SetActive(true);
        }
    }

    void triggerStop()
    {
        /*
        if (startEvent)
        {
            GameObject player = GameObject.FindGameObjectWithTag("player");
            player.SendMessage("startEvent", eventToStart);
        }*/
        if (targetLight1 != null)
        {
            targetLight1.SetActive(false);
        }
        if (targetLight2 != null)
        {
            targetLight2.SetActive(false);
        }
    }
    /*
    void OnGUI()
    {
        if (showText)
        {
            GUI.Label(new Rect(pointA.x + 5f, Screen.height - pointA.y - 43f, 100f, 100f), "Press E");
        }
    }*/
}
