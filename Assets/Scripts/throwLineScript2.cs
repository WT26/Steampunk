using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class throwLineScript2 : MonoBehaviour
{
    //public GameObject gun; // up to you to initialize this to the gun

    void Start()
    {/*
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        //lineRenderer.SetWidth(0.2f, 0.2f);
        lineRenderer.numPositions = 2;
        lineRenderer.useWorldSpace = false;

        //lineRenderer.SetVertexCount(2);*/
    }

    void Update()
    {
        
    }
    // Update is called once per frame

    void OnGUI()
    {/*
        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Rigidbody2D player = GetComponentInParent<Rigidbody2D>();
        Vector2 middleOfTheScreen = Camera.main.WorldToScreenPoint(player.transform.position);

        float deltaX = middleOfTheScreen.x - Input.mousePosition.x;
        float deltaY = middleOfTheScreen.y - Input.mousePosition.y;

        //middleOfTheScreen.y = Screen.height - 100f - 43f;

        Rect rect = new Rect(deltaX, deltaY, 0.3f, 0.3f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.red);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(rect, GUIContent.none);

        //lineRenderer.SetPosition(0, middleOfTheScreen);
        //lineRenderer.SetPosition(1, Input.mousePosition);
        print("MOUSE POS: " + Input.mousePosition + "   PLAYER POS: " + middleOfTheScreen);*/
    }
}