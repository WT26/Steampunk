using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject canvas;
    public GameObject player;
    public GameObject checkpoints;
    public GameObject npcs;

    public Text health;
    public Text sanity;
    public Text selection;

    private string sanityString;
    private string healthString;
    private string selectionString;

    private int currentSanity;
    private int currentHealth;
    private int maxSanity;
    private int maxHealth;

    private int zombieToeCount;
    private int syringeCount;

    private int currentCheckpoint;
    private Vector3[] checkpointPositions;
    private GameObject[] npcGos;

    private bool pauseMenuOn;
    private bool invOn;

    private int selectedItem; // 0 = käsi, 1 = huuto?, 2 = kolmasvaihtoehto;
    private int cursorOnSelection; // 0 = käsi, 1 = huuto?, 2 = kolmasvaihtoehto;

    public enum IDs { NONE, ROCK, CLOCK, DUCK, BUCKET, CACTUS };
    public enum consumables { NONE, SYRINGE, ZOMBIETOE };

    private IDs[] currentlyHolding;

    // Use this for initialization
    void Start () {
        /*
        Button btn = quitButton.GetComponent<Button>();
        btn.onClick.AddListener(quitOnClick);

        Button btn2 = resumeButton.GetComponent<Button>();
        btn2.onClick.AddListener(resumeOnClick);

        */
        initializeGame();
    }

    // Update is called once per frame
    void Update () {

        //updatePlayer();

        //updateUi();
        //updateGameStatus();
        //updateInvSelection();

        //checkInputs();
    }

    private void updatePlayer()
    {
        currentHealth = player.GetComponent<playerMovement>().getHealth();
        currentSanity = player.GetComponent<playerMovement>().getSanity();
        currentlyHolding = player.GetComponent<playerMovement>().getCurrentlyHolding();
        zombieToeCount = player.GetComponent<playerMovement>().getZombieToeCount();
        syringeCount = player.GetComponent<playerMovement>().getSyringeCount();


    }
    /*
    private void checkInputs()
    {
        if (!pauseMenuOn)
        {
            if (Input.GetKeyDown("escape"))
            {
                togglePause();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                if (invOn)
                {
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    player.SendMessage("pauseMenuChanged", false);
                }
                else
                {
                    itemUi.SetActive(true);
                    invOn = true;
                    Time.timeScale = 0.2F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    player.SendMessage("pauseMenuChanged", true);
                }
            }
            else if (invOn && Input.GetButtonDown("Fire1"))
            {
                if (cursorOnSelection == 1)
                {
                    selectedItem = 1;
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    player.SendMessage("currentSelection", selectedItem);
                }
                else if (cursorOnSelection == 2)
                {
                    selectedItem = 2;
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    player.SendMessage("currentSelection", selectedItem);
                }
                else if (cursorOnSelection == 3)
                {
                    selectedItem = 3;
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    player.SendMessage("currentSelection", selectedItem);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown("escape"))
            {
                togglePause();
            }
        }
    }
    */
    private void updateGameStatus()
    {
        if (!checkIfAlive())
        {
            player.transform.position = checkpointPositions[currentCheckpoint];
        }
    }

    private void updateUi()
    {
        //sanityString = "SANITY: " + currentSanity + "/" + maxSanity;
        //healthString = "HEALTH: " + currentHealth + "/" + maxHealth;
        healthString = "";
        sanityString = "";
        /*
        selectionString = "\n\n\nSELECTION: " + selectedItem
            + "\nSLOT 1: " + currentlyHolding[0].ToString()
            + "\nSLOT 2: " + currentlyHolding[1].ToString()
            + "\nSLOT 3: " + currentlyHolding[2].ToString()
            + "\nSyringe Count: " + syringeCount
            + "\nPress right mouse button to open inv\nPress 'Esc' to quit";
            */
        health.text = healthString;
        sanity.text = sanityString;
        selection.text = "";
    }

    void initializeGame ()
    {
        currentSanity = 10;
        currentHealth = 10;
        maxSanity = 10;
        maxHealth = 10;
        currentCheckpoint = 0;
        checkpointPositions = new Vector3[10];
        npcGos = new GameObject[50];

        pauseMenuOn = false;
        invOn = false;
        selectedItem = 0;
        cursorOnSelection = 0;

        int i = 1;
        /*
        // Etsitään Checkpoints GO:n lapset ja otetaan niistä talteen checkpointtien sijainnit.
        foreach (Transform child in checkpoints.transform)
        {
            checkpointPositions[i] = child.position;
            i++;
        }*/
        i = 1;

        /*foreach (GameObject child in npcGos)
        {
            npcGos[i] = child;
            i++;
        }*/
        //player.transform.position = checkpointPositions[currentCheckpoint];
    }

    bool checkIfAlive ()
    {
        if (currentHealth < 0)
        {
            return false;
        }
        if (currentSanity < 0)
        {
            return false;
        }
        return true;
    }
    /*
    void quitOnClick()
    {
        Application.Quit();
    }
    
    void resumeOnClick()
    {
        togglePause();
    }

    void togglePause()
    {
        if (!pauseMenuOn)
        {
            pausemenu.SetActive(true);
            pauseMenuOn = true;
            Time.timeScale = 0.0001F;
            Time.fixedDeltaTime = 0.0001F * Time.timeScale;
            player.SendMessage("pauseMenuChanged", true);
        }
        else
        {
            pausemenu.SetActive(false);
            pauseMenuOn = false;
            Time.timeScale = 1.0F;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            player.SendMessage("pauseMenuChanged", true);
        }
    }

    void updateInvSelection()
    {
        if (invOn)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(player.transform.position);
            float distance = Vector3.Distance(mousePos, objectPos);

            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            


            foreach (Transform child in itemUi.transform)
            {
                if (child.tag == "itemui")
                {
                    itemUiChildren.Add(child.gameObject);
                }
            }
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            
            if (distance < 20)
            {
                itemUiChildren[0].SetActive(true);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                cursorOnSelection = 0;
            }
            else if (33 > angle && angle > -87)
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(true);
                cursorOnSelection = 1;
            }
            else if ((-87 > angle && angle > -180) || (angle > 144 && angle < 180))
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(true);
                itemUiChildren[3].SetActive(false);
                cursorOnSelection = 2;
            }
            else if (33 < angle && angle < 144)
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(true);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                cursorOnSelection = 3;
            }
        }
    }*/
    void currentSelection(int selection)
    {
        selectedItem = selection;
    }
}

