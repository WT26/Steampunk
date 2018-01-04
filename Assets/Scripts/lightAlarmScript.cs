using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAlarmScript : MonoBehaviour {



    private int frameCounter;
    private float startingRange;
    private Light spotlight;
    public bool isRightLight = true;

    public enum States { TURNEDOFF, SPINNING };
    public States currentState = States.TURNEDOFF;

    private float yValue;
    private float zValue;

    // Use this for initialization
    void Start () {
        frameCounter = 0;
        spotlight = this.GetComponent<Light>();
        //print(spotlight.range);
        startingRange = spotlight.range;

        if (isRightLight)
        {
            yValue = 90F;
            zValue = 0;
        }
        else
        {
            yValue = -90F;
            zValue = 0;
        }
    }

    // Update is called once per frame
    void Update () {

        switch (currentState)
        {
            case States.TURNEDOFF:
                spotlight.enabled = false;
                break;
            case States.SPINNING:
                spotlight.enabled = true;
                //spotlight.transform.localEulerAngles = new Vector3(spotlight.transform.localEulerAngles.x, 74.5F, 69.8F);
                //spotlight.transform.Rotate(Vector3.left * Time.deltaTime * speed);
                float angle = frameCounter;

                if (!isRightLight)
                {
                    angle = -angle;
                }

                spotlight.transform.rotation = Quaternion.Euler(angle, yValue, zValue);


                frameCounter++;
                frameCounter++;

                if (frameCounter > 360)
                {
                    frameCounter = 0;
                }

                //spotlight.transform.Rotate(1 * Time.deltaTime, transform.localEulerAngles.x, transform.localEulerAngles.z); 
                break;

            default:
                break;
        }

    }

    public void toggleTurnedOn()
    {
        if (currentState == States.TURNEDOFF)
        {
            currentState = States.SPINNING;
        }
        else
        {
            currentState = States.TURNEDOFF;
        }

    }

}
