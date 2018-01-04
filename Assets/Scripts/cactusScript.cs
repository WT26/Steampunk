using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cactusScript : MonoBehaviour
{
    private bool showText;
    //private RectTransform imageRectTransform;

    private Vector3 pointA;
    private Vector3 pointB;

    private GameObject player;

    public int punchForce = 250;

    private bool justHit = false;

    public int timeActive = 3;
    public bool ringing = false;

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "zombie")
        {
            other.gameObject.SendMessage("eatCactus");
            /*
            Rigidbody2D ownVelo = gameObject.GetComponent<Rigidbody2D>();
            if (ownVelo.velocity.x > 3f || ownVelo.velocity.y > 3f)
            {
                other.gameObject.SendMessage("applyDamage", 1);
                Rigidbody2D rbHuman = other.gameObject.GetComponent<Rigidbody2D>();
                rbHuman.AddForce(transform.forward.normalized * punchForce);
                ownVelo.velocity = new Vector3(0f, 0f, 0f);
            }
            */
            //Destroy(gameObject);

        }
        else if (other.gameObject.tag == "human")
        {
            /*
            Rigidbody2D ownVelo = gameObject.GetComponent<Rigidbody2D>();
            if (ownVelo.velocity.x > 3f || ownVelo.velocity.y > 3f)
            {
                other.gameObject.SendMessage("applyDamage", 1);
                Rigidbody2D rbHuman = other.gameObject.GetComponent<Rigidbody2D>();
                rbHuman.AddForce(transform.forward.normalized * punchForce);
                ownVelo.velocity = new Vector3(0f, 0f, 0f);
            }*/
        }
        else if (other.gameObject.tag == "player")
        {
            //other.gameObject.SendMessage("applyDamage", 1);
            //Destroy(gameObject);
        }
        else if (other.gameObject.tag == "bullet")
        {
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        if (showText)
        {
            GUI.Label(new Rect(pointA.x + 5f, Screen.height - pointA.y - 43f, 100f, 100f), "Click to pick up");
        }
    }

    void pickedUp()
    {
        showText = false;
        Destroy(gameObject);
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {


        /*
    GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 1f;
        lr.endWidth = 5f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Material lineMaterial = new Material(Shader.Find(" Diffuse"));
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;

        lr.material = lineMaterial;
        GameObject.Destroy(myLine, duration);*/






        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.SetPosition(0, start);
        //lineRenderer.SetPosition(1, end);

        //lineRenderer.startWidth = 1f;
        //lineRenderer.endWidth = 5f;
        //lineRenderer.SetPosition(1, end);

        /*
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var t = Time.time;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            
        }*/
    }
    public void pickingUp()
    {
        player.SendMessage("lift", GameController.IDs.CACTUS);
    }

    public void startRing()
    {
        ringing = true;
        StartCoroutine(ringingTimer());
    }

    IEnumerator ringingTimer()
    {
        yield return new WaitForSeconds(timeActive);
        ringing = false;
    }
}
