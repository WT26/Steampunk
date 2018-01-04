using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danceAreaScript : MonoBehaviour {

    public bool active = false;

	// Use this for initialization
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (active)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = true;
        }
        else
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.enabled = false;

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "human")
        {
            print("asd");
            other.SendMessage("startDancing");
        }
    }
    public void activate()
    {
        active = true;
    }

    public void deactivate()
    {
        active = false;
    }
}
