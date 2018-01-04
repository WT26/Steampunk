using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingTargetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        other.BroadcastMessage("targetReached");
    }
}
