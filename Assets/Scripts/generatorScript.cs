using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatorScript : MonoBehaviour {


    public bool pressed;
    public doorScript targetDoor;
    public doorScript targetDoor2;
    public doorScript targetDoor3;
    public doorScript targetDoor4;
    public zombieSpawnScript targetZombieSpawn1;

    public loudspeakerScript targetLoudspeaker;

    public lightAlarmScript targetLightAlarm1;
    public lightAlarmScript targetLightAlarm2;
    public bool playAlarmSound = false;


    public bool startEvent = false;
    public int eventToStart = 0;
    private bool eventPlayed = false;

    public bool clickableOnce = false;
    private int clickedCount = 0;

    private bool showText;
    //public RectTransform imageRectTransform;

    private Vector3 pointA;
    private Vector3 pointB;

    // Use this for initialization
    void Start()
    {
        pressed = false;
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
            pointA.x = pointA.x + 10;
            pointA.y = pointA.y + 10;
            pointB = pointA;
            pointB.x = pointB.x + 25;
            pointB.y = pointB.y + 20;

            Vector3 differenceVector = pointB - pointA;

            int lineWidth = 2;
            /*
            imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            imageRectTransform.pivot = new Vector2(0, 0.5f);
            imageRectTransform.position = pointA;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
            */

            if (Input.GetButtonDown("Use"))
            {
                if (clickableOnce && clickedCount > 0)
                {
                }
                else
                {
                    GameObject player = GameObject.FindGameObjectWithTag("player");
                    player.SendMessage("playOnce", playerMovement.allSfx.NAPPIPAINALLUS);
                    clickedCount++;
                    if (!eventPlayed && startEvent)
                    {
                        player.SendMessage("startEvent", eventToStart);
                        eventPlayed = true;
                    }

                    if (targetDoor != null)
                    {
                        targetDoor.toggleDoor();
                    }
                    if (targetDoor2 != null)
                    {
                        targetDoor2.toggleDoor();
                    }
                    if (targetDoor3 != null)
                    {
                        targetDoor3.toggleDoor();
                    }
                    if (targetDoor4 != null)
                    {
                        targetDoor4.toggleDoor();
                    }
                    if (targetLoudspeaker != null)
                    {
                        targetLoudspeaker.toggleSpeaker();
                    }

                    if (targetLightAlarm1 != null)
                    {
                        targetLightAlarm1.toggleTurnedOn();

                    }
                    if (targetLightAlarm2 != null)
                    {
                        targetLightAlarm2.toggleTurnedOn();
                    }
                    if (playAlarmSound)
                    {
                        GameObject playerr = GameObject.FindGameObjectWithTag("player");
                        playerr.SendMessage("playOnce", playerMovement.allSfx.ALARM);
                    }
                    if (targetZombieSpawn1 != null)
                    {
                        targetZombieSpawn1.SendMessage("activateSpawn");
                    }
                }
            }
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
            GUI.Label(new Rect(pointA.x + 5f, Screen.height - pointA.y - 43f, 100f, 100f), "Press E to start Generator");
        }
    }
}
