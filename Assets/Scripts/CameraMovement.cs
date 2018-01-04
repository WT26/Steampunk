using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Transform target;

    public float smoothTime = 0.5F;
    public float maxSpeed = 1000F;
    public bool following = true;
    private Vector3 velocity = Vector3.zero;
    
    private bool usingStrictCamera = true;
    // Use this for initialization
    void Start () {
        target = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        // Original

        if (following)
        {
            if (usingStrictCamera)
            {
                GetComponent<Transform>().position = target.position + new Vector3(0, 0, -10);
            }
            else
            {
                Vector3 posizion = target.position + new Vector3(0, 0, -10);
                transform.position = Vector3.SmoothDamp(transform.position, posizion, ref velocity, smoothTime, maxSpeed, Time.deltaTime);
            }
        }
    }

    public void setTarget(GameObject Go)
    {
        target = Go.transform;
    }

    public void useStrictCamera(bool yesOrNoy)
    {
        usingStrictCamera = yesOrNoy;
    }
    public void stopFollowing()
    {
        following = false;
    }
    public void startFollowing()
    {
        following = true;
    }
}
