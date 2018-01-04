using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    //private GameObject[] zombies;
    //private GameObject player;

    //private int wall;

    private float xDiff;
    private float yDiff;
    public int punchForce = 250;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 1);
        //wall = 1 << 8;

        //zombies = GameObject.FindGameObjectsWithTag("zombie");
        //player = GameObject.FindGameObjectWithTag("player");

    }

    void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "zombie")
        {
            other.gameObject.SendMessage("applyDamage", 1);

            Rigidbody2D rbHuman = other.gameObject.GetComponent<Rigidbody2D>();
            rbHuman.AddForce(transform.forward.normalized * punchForce);
            Destroy(gameObject);

        }
        else if(other.gameObject.tag == "player")
        {
            other.gameObject.SendMessage("applyDamage", 1);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "bullet")
        {
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
