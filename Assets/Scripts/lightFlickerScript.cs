using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlickerScript : MonoBehaviour {


    private int frameCounter;
    private float startingRange;
    private Light spotlight;

    public enum States { IDLE, FLICKER, SNAP, WAVE };
    public States currentState = States.IDLE;


    
    void Start () {
        frameCounter = 0;
        spotlight = this.GetComponent<Light>();
        startingRange = spotlight.range;
	}
	
	void Update () {
		


        switch (currentState)
        {
            case States.IDLE:

                break;
            case States.FLICKER:
                if (frameCounter < 80)
                {
                    break;
                }
                else if (frameCounter < 100)
                {
                    spotlight.range = spotlight.range - 0.05f;
                }
                else if (frameCounter < 105)
                {
                    spotlight.range = spotlight.range + 1f;
                }
                else if (frameCounter < 110)
                {
                    spotlight.range = spotlight.range - 1f;
                }
                else if (frameCounter < 115)
                {
                    spotlight.range = spotlight.range + 0.6f;
                }
                else if (frameCounter < 120)
                {
                    spotlight.range = startingRange;
                }

                else
                {
                    spotlight.range = spotlight.range - 0.1f;
                }
                break;

            case States.SNAP:

                if (frameCounter < 135)
                {
                    break;
                }
                else if (frameCounter < 165)
                {
                    spotlight.range = 0.01f;
                }
                else
                {
                    spotlight.range = startingRange;
                }
                break;

            case States.WAVE:
                if (frameCounter < 60)
                {
                    spotlight.range = spotlight.range - 0.10f;
                }
                else if( frameCounter < 120 )
                {
                    spotlight.range = spotlight.range + 0.20f;
                }
                else
                {
                    spotlight.range = spotlight.range - 0.1f;
                }
                break;

            default:
                break;
        }

        
        frameCounter++;
        if (frameCounter > 180)
        {
            spotlight.range = startingRange;
            currentState = States.IDLE;
            if (Random.Range(0, 10) > 7)
            {
                currentState = (States)Random.Range(0, 3);
            }
            frameCounter = 0;
        }
    }
}
