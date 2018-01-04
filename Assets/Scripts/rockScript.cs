using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockScript : MonoBehaviour {


    private bool showText;
    //private RectTransform imageRectTransform;

    private Vector3 pointA;
    private Vector3 pointB;

    private GameObject player;

    public int punchForce = 250;

    private bool justHit = false;
    //private enum IDs { NONE, ROCK };


    //public Color c1 = Color.yellow;
    //public Color c2 = Color.red;
    //public int lengthOfLineRenderer = 2;


    //LineRenderer lineRenderer;


    // Use this for initialization
    void Start () {
        //pressed = false;
        showText = false;

        pointA = transform.position;
        pointB = transform.position;


        
        /*
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        //lineRenderer.widthMultiplier = 0.1f;
        //lineRenderer.startWidth = 0.15f;
        //lineRenderer.endWidth = 0.3f;
        lineRenderer.numPositions = lengthOfLineRenderer;

        */
        //lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        
        /*
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;*/
    }
	
	// Update is called once per frame
	void Update () {
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
            /*
            Vector3 drawPointA = transform.position + new Vector3(0.5f, 0.5f, 0);
            Vector3 drawPointB = transform.position + new Vector3(1f, 1f, 0);
            DrawLine(drawPointA, drawPointB, Color.red, 10);
            */
            /*
            int lineWidth = 2;

            Vector3 drawPointA = transform.position + new Vector3(0.5f, 0.5f, 0);
            Vector3 drawPointB = transform.position + new Vector3(1f, 1f, 0);

            //print("Drawpoint: " + drawPointA);
            //print(transform.position);
            DrawLine(drawPointA, drawPointB, Color.red, 10);
            */


            /*
            imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            imageRectTransform.pivot = new Vector2(0, 0.5f);
            imageRectTransform.position = pointA;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
            */

            
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
            Rigidbody2D ownVelo = gameObject.GetComponent<Rigidbody2D>();
            if (ownVelo.velocity.x > 3f || ownVelo.velocity.y > 3f)
            {
                other.gameObject.SendMessage("applyDamage", 1);
                Rigidbody2D rbHuman = other.gameObject.GetComponent<Rigidbody2D>();
                rbHuman.AddForce(transform.forward.normalized * punchForce);
                ownVelo.velocity = new Vector3(0f, 0f, 0f);
            }
            
            //Destroy(gameObject);

        }
        else if (other.gameObject.tag == "human")
        {
            Rigidbody2D ownVelo = gameObject.GetComponent<Rigidbody2D>();
            if (ownVelo.velocity.x > 3f || ownVelo.velocity.y > 3f)
            {
                other.gameObject.SendMessage("applyDamage", 1);
                Rigidbody2D rbHuman = other.gameObject.GetComponent<Rigidbody2D>();
                rbHuman.AddForce(transform.forward.normalized * punchForce);
                ownVelo.velocity = new Vector3(0f, 0f, 0f);
            }
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
        player.SendMessage("lift", GameController.IDs.ROCK);
    }
}
