using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class playerMovement : MonoBehaviour
{

    public int health = 8;
    public int maxHealth = 8;
    public int sanity = 8;
    public int maxSanity = 8;
    public int syringeCount = 0;
    public int zombieToeCount = 0;

    public float speed;

    private Animator animator;

    //public float acceleration = 5;
    //public float maxAcceleration = 100;

    public bool useController;

    public GameController.IDs holdingSlot1;
    public GameController.IDs holdingSlot2;
    public GameController.IDs holdingSlot3;
    public GameController.IDs currentlyOnHand = GameController.IDs.NONE;

    private GameObject itemOnFloor;

    public Rigidbody2D rock;
    public Rigidbody2D alarmClock;
    public Rigidbody2D cactus;
    public Rigidbody2D duck;
    public Rigidbody2D bucket;

    public Texture2D cursorTexture;

    private bool collidingItemOnFloor = false;
    private bool gamePaused = false;

    private bool healthChanged = true;
    private bool selectedItemChanged = true;

    private float distanceToGo = 1.0f;

    // Game controlling vars
    private int restartButtonCounter = 0;
    private bool pauseMenuOn;
    private bool invOn;
    private int selectedItem; // 0 = käsi, 1 = huuto?, 2 = kolmasvaihtoehto;
    private int cursorOnSelection; // 0 = käsi, 1 = huuto?, 2 = kolmasvaihtoehto;

    private GameObject mainCamera;
    private GameObject gamestate;
    public GameObject pausemenu;
    public GameObject itemUi;
    public GameObject healthBarUi;
    public GameObject selectedItemUi;
    public GameObject fadeBlack;
    public GameObject upperFocusGo;
    public GameObject lowerFocusGo;
    public GameObject newspaperUi;
    public GameObject restartLevelUi;

    private Image fadeToBlack;
    private Image upperFocusImage;
    private Image lowerFocusImage;

    public List<GameObject> itemUiChildren;
    private List<GameObject> syringeTexts;
    private List<GameObject> zombieToeTexts;
    public List<GameObject> healthBarUiChildren;
    public List<GameObject> selectedItemUiChildren;
    public List<GameObject> newspaperUiChildren;

    private bool newspaperShowing = false;
    private int showingNewspaperId = 1;

    public Button quitButton;
    public Button resumeButton;

    private GameObject pickups;

    // SFX
    public enum allSfx { NONE, IDLEOLINA, ZOMBIEAU, ALARM, LINNUT, RADIOKOHINA, NAPPIPAINALLUS, OVINARINA,
    SWITCH, BITE, GUARDNOTICE, IHMISJOUKKO, SHOTGUN };

    private AudioSource soundPlayer;
    public AudioClip idleOlina;
    public AudioClip zombieAu;
    public AudioClip alarm;
    public AudioClip linnut;
    public AudioClip radiokohina;
    public AudioClip gunshot1;
    public AudioClip gunshot2;
    public AudioClip nappiPainallus;
    public AudioClip oviNarina;
    public AudioClip switch1;
    public AudioClip bite;
    public AudioClip guardNotice;
    public AudioClip ihmisjoukko;
    public AudioClip shotgun;

    private bool soundPlayed = false;


    // EVENT SYSTEM

    public int currentLevel = 2;
    private bool eventOn = false;
    private int eventToProceed = 0;

    private int eventFrameCounter1 = 0;
    private int eventFrameCounter2 = 0;
    private int eventFrameCounter3 = 0;
    private int eventFrameCounter4 = 0;
    private int eventFrameCounter5 = 0;

    private bool eventTempBool1 = false;
    private bool eventTempBool2 = false;
    private bool eventTempBool3 = false;
    private bool eventTempBool4 = false;
    private bool eventTempBool5 = false;

    public GameObject eventCameraTarget1;
    public GameObject eventCameraTarget2;
    public GameObject eventCameraTarget3;
    public GameObject eventCameraTarget4;

    public GameObject eventZombieSpawn1;

    public GameObject eventAlarmLight1;
    public GameObject eventAlarmLight2;

    public GameObject eventLoudspeaker1;

    public GameObject eventHuman1;
    public GameObject eventHuman2;

    public GameObject eventGo1;
    public GameObject eventGo2;




    void Start()
    {
        Application.targetFrameRate = 60;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        animator = GetComponent<Animator>();

        //Vector2 hotSpot = Vector2.zero;
        Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        CursorMode cursorMode = CursorMode.Auto;
        Cursor.SetCursor(cursorTexture, cursorHotspot, cursorMode);

        pauseMenuOn = false;
        invOn = false;
        selectedItem = 1;
        cursorOnSelection = 0;

        //Button btn = quitButton.GetComponent<Button>();
        //btn.onClick.AddListener(quitOnClick);

        Button btn2 = resumeButton.GetComponent<Button>();
        btn2.onClick.AddListener(resumeOnClick);

        gamestate = GameObject.FindWithTag("gameController");

        pickups = GameObject.FindGameObjectWithTag("pickup");

        syringeTexts = new List<GameObject>();
        zombieToeTexts = new List<GameObject>();

        foreach (Transform child in itemUi.transform)
        {
            if (child.tag == "itemui")
            {
                itemUiChildren.Add(child.gameObject);
                foreach (Transform y in child.transform)
                {
                    if (y.tag == "syringeText")
                    {
                        Text syringeText = y.GetComponent<Text>();
                        syringeText.text = syringeCount.ToString();
                        syringeTexts.Add(y.gameObject);

                    }
                    else if (y.tag == "zombieToeText")
                    {
                        Text zombieToeText = y.GetComponent<Text>();
                        zombieToeText.text = zombieToeCount.ToString();
                        zombieToeTexts.Add(y.gameObject);
                    }
                }
            }
        }

        foreach (Transform child in healthBarUi.transform)
        {
            if (child.tag == "healthui")
            {
                healthBarUiChildren.Add(child.gameObject);
            }
        }

        foreach (Transform child in selectedItemUi.transform)
        {
            selectedItemUiChildren.Add(child.gameObject);
        }

        foreach (Transform child in newspaperUi.transform)
        {
            newspaperUiChildren.Add(child.gameObject);
        }

        Time.timeScale = 1.0F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        soundPlayer = GetComponent<AudioSource>();


        fadeToBlack = fadeBlack.GetComponent<Image>();
        Color c = fadeToBlack.color;
        c.a = 1;
        fadeToBlack.color = c;

        upperFocusImage = upperFocusGo.GetComponent<Image>();
        lowerFocusImage = lowerFocusGo.GetComponent<Image>();


        if (currentLevel == 1)
        {
            startEvent(1);
        }
        else if (currentLevel == 2)
        {
            startEvent(7);
        }
        else if (currentLevel == 3)
        {
            startEvent(9);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!pauseMenuOn)
        {
            if (!eventOn)
            {
                rotatePlayer();
                updateInvSelection();

                if (healthChanged)
                {
                    updateHealthBar();
                }

                if (selectedItemChanged)
                {
                    updateSelectedItem();
                }
                updateGameStatus();
            }
            else
            {
                proceedEvent(eventToProceed);
            }
        }
        checkInputs();
        updateAnimations();
        resetVariables();
    }


    void checkInputs()
    {

        if (!eventOn)
        {
            if (newspaperShowing)
            {
                handleNewspaperInput();
            }
            else
            {
                handleMovementInputs();
            }
        }

        // PAUSE MENU TOGGLE
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            //togglePause();
        }

        if (!pauseMenuOn)
        {
            if (!eventOn)
            {
                if (newspaperShowing)
                {
                    handleNewspaperInput();
                }
                else
                {
                    if (!invOn)
                    {
                        handlePlayerActions();
                    }
                    handleInventoryMenu();
                }
            }
        }
    }


    void eatCorpse(int amount)
    {
        if (health < maxHealth)
        {
            health += amount;
            healthChanged = true;
        }
        if (sanity > 1)
        {
            sanity -= amount;
        }
    }
    void readBook(int amount)
    {
        if (sanity < maxSanity)
        {
            sanity += amount;
        }
    }

    void applyDamage(int amount)
    {
        health -= amount;
        healthChanged = true;
    }

    public int getHealth()
    {
        return health;
    }

    public int getSanity()
    {
        return sanity;
    }

    public int getZombieToeCount()
    {
        return zombieToeCount;
    }
    public int getSyringeCount()
    {
        return syringeCount;
    }

    public void isZombie(GameObject other)
    {
        if (sanity > 6)
        {
            other.SendMessage("isPlayerZombie", false);
        }
        else
        {
            other.SendMessage("isPlayerZombie", true);
        }
    }

    public GameController.IDs[] getCurrentlyHolding()
    {
        GameController.IDs[] holdingList = { holdingSlot1, holdingSlot2, holdingSlot3 };
        return holdingList;
    }

    void liftItem()
    {
        if (itemOnFloor.tag == "rock" || itemOnFloor.tag == "alarmClock" ||
            itemOnFloor.tag == "cactus" || itemOnFloor.tag == "duck" ||
            itemOnFloor.tag == "bucket")
        {
            itemOnFloor.SendMessage("pickingUp");
        }
        else if (itemOnFloor.tag == "zombieToe" || itemOnFloor.tag == "syringe" || itemOnFloor.tag == "newspaper")
        {
            itemOnFloor.SendMessage("pickingUp");
        }
    }

    public void lift(GameController.IDs id)
    {
        int openSlot = checkIfSlotsOpen();

        if (openSlot != 0)
        {
            if (id == GameController.IDs.ROCK)
            {
                itemOnFloor.SendMessage("pickedUp");
                StartCoroutine(itemPickedUpWait(GameController.IDs.ROCK, openSlot));
            }
            else if (id == GameController.IDs.CLOCK)
            {
                itemOnFloor.SendMessage("pickedUp");
                StartCoroutine(itemPickedUpWait(GameController.IDs.CLOCK, openSlot));
            }
            else if (id == GameController.IDs.CACTUS)
            {
                itemOnFloor.SendMessage("pickedUp");
                StartCoroutine(itemPickedUpWait(GameController.IDs.CACTUS, openSlot));
            }
            else if (id == GameController.IDs.DUCK)
            {
                itemOnFloor.SendMessage("pickedUp");
                StartCoroutine(itemPickedUpWait(GameController.IDs.DUCK, openSlot));
            }
            else if (id == GameController.IDs.BUCKET)
            {
                itemOnFloor.SendMessage("pickedUp");
                StartCoroutine(itemPickedUpWait(GameController.IDs.BUCKET, openSlot));
            }
        }
    }

    public void addConsumable(GameController.consumables consum)
    {
        
        if (consum == GameController.consumables.SYRINGE)
        {
            syringeCount += 1;
        }
        else if (consum == GameController.consumables.ZOMBIETOE)
        {
            zombieToeCount += 1;
        }
        itemOnFloor.SendMessage("pickedUp");
        StartCoroutine(consumableAdded());
    }

    public void takeNewspaper(int newspaperId)
    {

        newspaperUiChildren[newspaperId - 1].SetActive(true);
        newspaperUi.SetActive(true);
        newspaperShowing = true;
        showingNewspaperId = newspaperId;
        ScrollRect srs = newspaperUi.GetComponent<ScrollRect>();
        srs.content = newspaperUiChildren[newspaperId - 1].GetComponent<RectTransform>();
        itemOnFloor.SendMessage("pickedUp");
        StartCoroutine(consumableAdded());
    }

    public void consume(GameController.consumables consumable)
    {
        if (consumable == GameController.consumables.SYRINGE)
        {
            if (syringeCount > 0)
            {
                health -= 2;
                if (health < 1)
                {
                    health = 1;
                }

                sanity += 2;
                if (sanity > maxSanity)
                {
                    sanity = maxSanity;
                }
                syringeCount--;
            }
        }
        else if (consumable == GameController.consumables.ZOMBIETOE)
        {
            if (zombieToeCount > 0)
            {
                health += 2;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }

                sanity -= 2;
                if (sanity < 1)
                {
                    sanity = 1;
                }
                zombieToeCount--;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        itemOnFloor = other.gameObject;
        collidingItemOnFloor = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //itemOnFloor = other.gameObject;
        collidingItemOnFloor = false;
    }

    IEnumerator itemPickedUpWait(GameController.IDs item, int slot)
    {
        yield return new WaitForSeconds(0.01f);
        if (slot == 1)
        {
            holdingSlot1 = item;
        }
        else if (slot == 2)
        {
            holdingSlot2 = item;
        }
        else
        {
            holdingSlot3 = item;
        }
        selectedItemChanged = true;
        collidingItemOnFloor = false;
    }

    IEnumerator consumableAdded()
    {
        yield return new WaitForSeconds(0.01f);
        collidingItemOnFloor = false;
    }

    int checkIfSlotsOpen()
    {
        if (holdingSlot1 == GameController.IDs.NONE)
        {
            return 1;
        }
        else if (holdingSlot2 == GameController.IDs.NONE)
        {
            return 2;
        }
        else if (holdingSlot3 == GameController.IDs.NONE)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    void resetVariables()
    {
        soundPlayed = false;
        //collidingItemOnFloor = false;
    }

    void throwItem()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        Vector3 throwDir;
        if (angle > -130 && angle < -60)
        {
            throwDir = new Vector3(0, -0.7f, 0);
        }
        else if ((angle < -130 && angle > -180) || (angle < 180 && angle > 130))
        {
            throwDir = new Vector3(-0.7f, 0, 0);
        }
        else if (angle < 130 && angle > 30)
        {
            throwDir = new Vector3(0, 0.7f, 0);
        }
        else
        {
            throwDir = new Vector3(0.7f, 0, 0);
        }

        if (selectedItem == 1)
        {
            Rigidbody2D bul = new Rigidbody2D();
            if (holdingSlot1 == GameController.IDs.ROCK)
            {
                bul = Instantiate(rock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "rock";
            }
            else if (holdingSlot1 == GameController.IDs.CLOCK)
            {
                bul = Instantiate(alarmClock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "alarmClock";
                bul.SendMessage("startRing");
            }
            else if (holdingSlot1 == GameController.IDs.CACTUS)
            {
                bul = Instantiate(cactus, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "cactus";
            }
            else if (holdingSlot1 == GameController.IDs.DUCK)
            {
                bul = Instantiate(duck, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "duck";
            }
            else if (holdingSlot1 == GameController.IDs.BUCKET)
            {
                bul = Instantiate(bucket, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "bucket";
            }
            if (holdingSlot1 != GameController.IDs.NONE)
            {
                bul.transform.parent = pickups.transform;
                bul.velocity = transform.TransformDirection(0, -25, 0);
                holdingSlot1 = GameController.IDs.NONE;
            }
        }
        // SAME FOR 2 and 3 selections, TODO same recursively
        else if (selectedItem == 2)
        {
            Rigidbody2D bul = new Rigidbody2D();
            if (holdingSlot2 == GameController.IDs.ROCK)
            {
                bul = Instantiate(rock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "rock";
            }
            else if (holdingSlot2 == GameController.IDs.CLOCK)
            {
                bul = Instantiate(alarmClock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "alarmClock";
                bul.SendMessage("startRing");
            }
            else if (holdingSlot2 == GameController.IDs.CACTUS)
            {
                bul = Instantiate(cactus, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "cactus";
            }
            else if (holdingSlot2 == GameController.IDs.DUCK)
            {
                bul = Instantiate(duck, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "duck";
            }
            else if (holdingSlot2 == GameController.IDs.BUCKET)
            {
                bul = Instantiate(bucket, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "bucket";
            }
            if (holdingSlot2 != GameController.IDs.NONE)
            {
                bul.transform.parent = pickups.transform;
                bul.velocity = transform.TransformDirection(0, -25, 0);
                holdingSlot2 = GameController.IDs.NONE;
            }
        }
        else if (selectedItem == 3)
        {
            Rigidbody2D bul = new Rigidbody2D();
            if (holdingSlot3 == GameController.IDs.ROCK)
            {
                bul = Instantiate(rock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "rock";
            }
            else if (holdingSlot3 == GameController.IDs.CLOCK)
            {
                bul = Instantiate(alarmClock, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "alarmClock";
                bul.SendMessage("startRing");
            }
            else if (holdingSlot3 == GameController.IDs.CACTUS)
            {
                bul = Instantiate(cactus, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "cactus";
            }
            else if (holdingSlot3 == GameController.IDs.DUCK)
            {
                bul = Instantiate(duck, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "duck";
            }
            else if (holdingSlot3 == GameController.IDs.BUCKET)
            {
                bul = Instantiate(bucket, transform.position + throwDir, transform.rotation) as Rigidbody2D;
                bul.tag = "bucket";
            }
            if (holdingSlot3 != GameController.IDs.NONE)
            {
                bul.transform.parent = pickups.transform;
                bul.velocity = transform.TransformDirection(0, -25, 0);
                holdingSlot3 = GameController.IDs.NONE;
            }
        }
        selectedItemChanged = true;
    }

    void pauseMenuChanged(bool isPauseMenuOn)
    {
        gamePaused = isPauseMenuOn;
    }

    public void quitOnClick()
    {
        print("Quitting");
        Application.Quit();
    }

    public void resumeOnClick()
    {
        print("Resumee");
        togglePause();
    }

    public void togglePause()
    {
        if (!pauseMenuOn)
        {
            pausemenu.SetActive(true);
            pauseMenuOn = true;
            Time.timeScale = 0.0001F;
            Time.fixedDeltaTime = 0.0001F * Time.timeScale;
        }
        else
        {
            pausemenu.SetActive(false);
            pauseMenuOn = false;
            Time.timeScale = 1.0F;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    void rotatePlayer()
    {
        if (!useController)
        {
            // Use mouse.
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }
        else
        {
            // Using controller;
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("RHorizontal") + Vector3.forward * -Input.GetAxisRaw("RVertical");

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                if (playerDirection.x > 0)
                {
                    playerDirection.x = 0;
                    playerDirection.y = 0;
                    playerDirection.z = playerDirection.z * 90 + 90;
                }
                else
                {
                    playerDirection.x = 0;
                    playerDirection.y = 0;
                    playerDirection.z = -playerDirection.z * 90 + 90;
                }

                transform.rotation = Quaternion.Euler(playerDirection);
            }
        }
    }

    void updateInvSelection()
    {
        if (invOn)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            float distance = Vector3.Distance(mousePos, objectPos);

            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            int slot1_itemNumber = 0;
            int slot2_itemNumber = 0;
            int slot3_itemNumber = 0;


            if (holdingSlot1 == GameController.IDs.DUCK)
            {
                slot1_itemNumber = 3;
            }
            else if (holdingSlot1 == GameController.IDs.ROCK)
            {
                slot1_itemNumber = 8;
            }
            else if (holdingSlot1 == GameController.IDs.CACTUS)
            {
                slot1_itemNumber = 9;
            }
            else if (holdingSlot1 == GameController.IDs.BUCKET)
            {
                slot1_itemNumber = 14;
            }
            else if (holdingSlot1 == GameController.IDs.CLOCK)
            {
                slot1_itemNumber = 17;
            }

            if (holdingSlot2 == GameController.IDs.DUCK)
            {
                slot2_itemNumber = 4;
            }
            else if (holdingSlot2 == GameController.IDs.ROCK)
            {
                slot2_itemNumber = 7;
            }
            else if (holdingSlot2 == GameController.IDs.CACTUS)
            {
                slot2_itemNumber = 10;
            }
            else if (holdingSlot2 == GameController.IDs.BUCKET)
            {
                slot2_itemNumber = 13;
            }
            else if (holdingSlot2 == GameController.IDs.CLOCK)
            {
                slot2_itemNumber = 16;
            }

            if (holdingSlot3 == GameController.IDs.DUCK)
            {
                slot3_itemNumber = 5;
            }
            else if (holdingSlot3 == GameController.IDs.ROCK)
            {
                slot3_itemNumber = 6;
            }
            else if (holdingSlot3 == GameController.IDs.CACTUS)
            {
                slot3_itemNumber = 11;
            }
            else if (holdingSlot3 == GameController.IDs.BUCKET)
            {
                slot3_itemNumber = 12;
            }
            else if (holdingSlot3 == GameController.IDs.CLOCK)
            {
                slot3_itemNumber = 15;
            }

            foreach (GameObject uitext in syringeTexts)
            {

                Text syringeText = uitext.GetComponent<Text>();
                syringeText.text = syringeCount.ToString();
            }

            foreach (GameObject uitext in zombieToeTexts)
            {
                Text zombieToeText = uitext.GetComponent<Text>();
                zombieToeText.text = zombieToeCount.ToString();
            }


            //Text syringeCountText = itemUiChildren[4].GetComponentInChildren<Text>();
            //syringeCountText.text = syringeCount.ToString();

            //Text zombieToeCountText = itemUiChildren[5].GetComponentInChildren<Text>();
            //zombieToeCountText.text = zombieToeCount.ToString();

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            if (distance < 50)
            {
                itemUiChildren[0].SetActive(true);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(false);

                int i = 0;
                foreach (Transform child in itemUiChildren[0].transform)
                {
                    if (i < 3 || i == slot3_itemNumber || i == slot2_itemNumber || i == slot1_itemNumber)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    i++;
                }

                cursorOnSelection = 0;
            }
            else if ((-141 > angle && angle > -180) || (angle > 152 && angle < 180))
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(true);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(false);

                int i = 0;
                foreach (Transform child in itemUiChildren[1].transform)
                {
                    if (i < 3 || i == slot3_itemNumber || i == slot2_itemNumber || i == slot1_itemNumber)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    i++;
                }

                cursorOnSelection = 1;
            }
            else if ((angle < -45 && angle > -141))
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(true);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(false);

                int i = 0;
                foreach (Transform child in itemUiChildren[2].transform)
                {
                    if (i < 3 || i == slot3_itemNumber || i == slot2_itemNumber || i == slot1_itemNumber)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    i++;
                }

                cursorOnSelection = 2;
            }
            else if (-45 < angle && angle < 25)
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(true);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(false);

                int i = 0;
                foreach (Transform child in itemUiChildren[3].transform)
                {
                    if (i < 3 || i == slot3_itemNumber || i == slot2_itemNumber || i == slot1_itemNumber)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    i++;
                }

                cursorOnSelection = 3;
            }
            else if (38 < angle && angle < 139)
            {
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(true);
                itemUiChildren[5].SetActive(false);

                int i = 0;
                foreach (Transform child in itemUiChildren[4].transform )
                {
                    if (i < 3 || i == slot3_itemNumber || i == slot2_itemNumber || i == slot1_itemNumber)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    i++;
                }

                cursorOnSelection = 4;
            }
            else if (90 < angle && angle < 139)
            {
                /* NOT IN USE FIFTH ITEM
                itemUiChildren[0].SetActive(false);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(true);
                cursorOnSelection = 5;
                */
            }
            else
            {
                itemUiChildren[0].SetActive(true);
                itemUiChildren[1].SetActive(false);
                itemUiChildren[2].SetActive(false);
                itemUiChildren[3].SetActive(false);
                itemUiChildren[4].SetActive(false);
                itemUiChildren[5].SetActive(false);

                cursorOnSelection = 0;
            }
        }
    }

    void updateHealthBar()
    {
        for (int i = 0; i < healthBarUiChildren.Count; i++)
        {
            if (i != health)
            {
                healthBarUiChildren[i].SetActive(false);
            }
            else
            {
                healthBarUiChildren[i].SetActive(true);
            }
        }
        healthChanged = false;
    }

    // Updates the selected item ui.
    void updateSelectedItem()
    {
        updateCurrentlyOnHand();
        for (int i = 0; i < selectedItemUiChildren.Count; i++)
        {
            selectedItemUiChildren[i].SetActive(false);
        }

        if (currentlyOnHand == GameController.IDs.NONE)
        {
            selectedItemUiChildren[0].SetActive(true);
        }
        else if (currentlyOnHand == GameController.IDs.CLOCK)
        {
            selectedItemUiChildren[0].SetActive(true);
            selectedItemUiChildren[1].SetActive(true);
        }
        else if (currentlyOnHand == GameController.IDs.BUCKET)
        {
            selectedItemUiChildren[0].SetActive(true);
            selectedItemUiChildren[2].SetActive(true);
        }
        else if (currentlyOnHand == GameController.IDs.CACTUS)
        {
            selectedItemUiChildren[0].SetActive(true);
            selectedItemUiChildren[3].SetActive(true);
        }
        else if (currentlyOnHand == GameController.IDs.ROCK)
        {
            selectedItemUiChildren[0].SetActive(true);
            selectedItemUiChildren[4].SetActive(true);
        }
        else if (currentlyOnHand == GameController.IDs.DUCK)
        {
            selectedItemUiChildren[0].SetActive(true);
            selectedItemUiChildren[5].SetActive(true);
        }
        selectedItemChanged = false;
    }

    void handleMovementInputs()
    {
        if (!useController)
        {
            // MOVEMENT

            // NW
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                //transform.position += Vector3.left * speed;
                //transform.position += Vector3.up * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);

            }
            // SW
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                //transform.position += Vector3.left * speed;
                //transform.position += Vector3.down * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed);

            }
            // SE
            else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                //transform.position += Vector3.right * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed);
                //transform.position += Vector3.down * speed;
            }
            // NE
            else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                //transform.position += Vector3.right * speed;
                //transform.position += Vector3.up * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
            }

            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                // Alternative way to move right.
                //transform.Translate(Vector2.right * speed);
                //GetComponent<Rigidbody2D>().AddForce(Vector2.right);

                //transform.position += Vector3.right * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                //transform.position += Vector3.left * speed;
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                //transform.position += Vector3.up * speed;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed);
                //transform.position += Vector3.down * speed;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                restartButtonCounter++;
                restartLevelUi.SetActive(true);
                StartCoroutine(restartRestartCounter());
                if (restartButtonCounter > 2)
                {
                    restartLevel();
                }
                //transform.position += Vector3.down * speed;
            }
        }
        // Controller
        else
        {
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * -Input.GetAxisRaw("Vertical");
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                playerDirection.y = -playerDirection.z;
                playerDirection.z = 0;
                print(playerDirection);
                GetComponent<Rigidbody2D>().AddForce(playerDirection * speed);
                //transform.rotation = Quaternion.Euler(playerDirection);
            }
        }
    }
    void handleInventoryMenu()
    {
        // INVENTORY MENU
        if (Input.GetButtonDown("Fire2"))
        {
            if (invOn)
            {
                itemUi.SetActive(false);
                invOn = false;
                Time.timeScale = 1.0F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                //player.SendMessage("pauseMenuChanged", false);
            }
            else
            {
                itemUi.SetActive(true);
                invOn = true;
                Time.timeScale = 0.2F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                //player.SendMessage("pauseMenuChanged", true);
            }
        }
        else if (invOn && Input.GetButtonDown("Fire1"))
        {
            if (cursorOnSelection == 1)
            {
                selectedItem = 1;
                selectedItemChanged = true;
                itemUi.SetActive(false);
                invOn = false;
                Time.timeScale = 1.0F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                gamestate.SendMessage("currentSelection", selectedItem);
            }
            else if (cursorOnSelection == 2)
            {
                selectedItem = 2;
                selectedItemChanged = true;
                itemUi.SetActive(false);
                invOn = false;
                Time.timeScale = 1.0F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                gamestate.SendMessage("currentSelection", selectedItem);
            }
            else if (cursorOnSelection == 3)
            {
                selectedItem = 3;
                selectedItemChanged = true;
                itemUi.SetActive(false);
                invOn = false;
                Time.timeScale = 1.0F;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                gamestate.SendMessage("currentSelection", selectedItem);
            }
            else if (cursorOnSelection == 4)
            {
                consume(GameController.consumables.SYRINGE);
            }
            else if (cursorOnSelection == 5)
            {
                consume(GameController.consumables.ZOMBIETOE);
            }
        }
    }
    void handlePlayerActions()
    {

        // THROW
        if (Input.GetButtonDown("Fire1"))
        {
            if (collidingItemOnFloor)
            {
                liftItem();
            }
            else
            {
                if (selectedItem == 1 || selectedItem == 2 || selectedItem == 3)
                {
                    throwItem();
                }
                else if (selectedItem == 4)
                {
                    consume(GameController.consumables.SYRINGE);
                }
                else if (selectedItem == 5)
                {
                    consume(GameController.consumables.ZOMBIETOE);
                }
            }
        }
    }

    void handleNewspaperInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            if (showingNewspaperId == 1)
            {
                newspaperUiChildren[showingNewspaperId - 1].SetActive(false);
                newspaperShowing = false;
                newspaperUi.SetActive(false);

            }
            if (showingNewspaperId == 2)
            {
                newspaperUiChildren[showingNewspaperId - 1].SetActive(false);
                newspaperShowing = false;
                newspaperUi.SetActive(false);

            }
            if (showingNewspaperId == 3)
            {
                newspaperUiChildren[showingNewspaperId - 1].SetActive(false);
                newspaperShowing = false;
                newspaperUi.SetActive(false);

            }
            if (showingNewspaperId == 4)
            {
                newspaperUiChildren[showingNewspaperId - 1].SetActive(false);
                newspaperShowing = false;
                newspaperUi.SetActive(false);


            }
            if (showingNewspaperId == 5)
            {
                newspaperUiChildren[showingNewspaperId - 1].SetActive(false);
                newspaperShowing = false;
                newspaperUi.SetActive(false);

            }
        }
    }
    void playOnce(allSfx sound)
    {
        soundPlayed = true;
        float volume = 1 / distanceToGo / 4;
        print(volume);
        soundPlayer.volume = volume;

        if (soundPlayed)
        {
            //soundPlayer.Stop();
            //player.loop = true;
            if (sound == allSfx.IDLEOLINA)
            {
                soundPlayer.PlayOneShot(idleOlina);
            }
            else if (sound == allSfx.ZOMBIEAU)
            {
                soundPlayer.PlayOneShot(zombieAu);
            }
            else if (sound == allSfx.ALARM)
            {
                soundPlayer.PlayOneShot(alarm);
            }
            else if (sound == allSfx.LINNUT)
            {
                soundPlayer.PlayOneShot(linnut);
            }
            else if (sound == allSfx.NAPPIPAINALLUS)
            {
                soundPlayer.PlayOneShot(nappiPainallus);
            }
            else if (sound == allSfx.OVINARINA)
            {
                soundPlayer.PlayOneShot(oviNarina);
            }
            else if (sound == allSfx.RADIOKOHINA)
            {
                soundPlayer.PlayOneShot(radiokohina);
            }
            else if (sound == allSfx.SWITCH)
            {
                soundPlayer.PlayOneShot(switch1);
            }
            else if (sound == allSfx.BITE)
            {
                soundPlayer.PlayOneShot(bite);
            }
            else if (sound == allSfx.GUARDNOTICE)
            {
                soundPlayer.PlayOneShot(guardNotice);
            }
            else if (sound == allSfx.IHMISJOUKKO)
            {
                soundPlayer.PlayOneShot(ihmisjoukko);
            }
            else if (sound == allSfx.SHOTGUN)
            {
                soundPlayer.PlayOneShot(shotgun);
            }
            // PLAY SOUND
        }
        soundPlayed = true;
    }

    void updateCurrentlyOnHand()
    {
        if (selectedItem == 1)
        {
            if (holdingSlot1 == GameController.IDs.ROCK)
            {
                currentlyOnHand = GameController.IDs.ROCK;
            }
            else if (holdingSlot1 == GameController.IDs.CLOCK)
            {
                currentlyOnHand = GameController.IDs.CLOCK;
            }
            else if (holdingSlot1 == GameController.IDs.CACTUS)
            {
                currentlyOnHand = GameController.IDs.CACTUS;
            }
            else if (holdingSlot1 == GameController.IDs.DUCK)
            {
                currentlyOnHand = GameController.IDs.DUCK;
            }
            else if (holdingSlot1 == GameController.IDs.BUCKET)
            {
                currentlyOnHand = GameController.IDs.BUCKET;
            }
            else if (holdingSlot1 == GameController.IDs.NONE)
            {
                currentlyOnHand = GameController.IDs.NONE;
            }
        }
        // SAME FOR 2 and 3 selections, TODO same recursively
        else if (selectedItem == 2)
        {
            if (holdingSlot2 == GameController.IDs.ROCK)
            {
                currentlyOnHand = GameController.IDs.ROCK;
            }
            else if (holdingSlot2 == GameController.IDs.CLOCK)
            {
                currentlyOnHand = GameController.IDs.CLOCK;
            }
            else if (holdingSlot2 == GameController.IDs.CACTUS)
            {
                currentlyOnHand = GameController.IDs.CACTUS;
            }
            else if (holdingSlot2 == GameController.IDs.DUCK)
            {
                currentlyOnHand = GameController.IDs.DUCK;
            }
            else if (holdingSlot2 == GameController.IDs.BUCKET)
            {
                currentlyOnHand = GameController.IDs.BUCKET;
            }
            else if (holdingSlot2 == GameController.IDs.NONE)
            {
                currentlyOnHand = GameController.IDs.NONE;
            }
        }
        else if (selectedItem == 3)
        {
            if (holdingSlot3 == GameController.IDs.ROCK)
            {
                currentlyOnHand = GameController.IDs.ROCK;
            }
            else if (holdingSlot3 == GameController.IDs.CLOCK)
            {
                currentlyOnHand = GameController.IDs.CLOCK;
            }
            else if (holdingSlot3 == GameController.IDs.CACTUS)
            {
                currentlyOnHand = GameController.IDs.CACTUS;
            }
            else if (holdingSlot3 == GameController.IDs.DUCK)
            {
                currentlyOnHand = GameController.IDs.DUCK;
            }
            else if (holdingSlot3 == GameController.IDs.BUCKET)
            {
                currentlyOnHand = GameController.IDs.BUCKET;
            }
            else if (holdingSlot3 == GameController.IDs.NONE)
            {
                currentlyOnHand = GameController.IDs.NONE;
            }
        }
    }

    void updateAnimations()
    {

        if (GetComponent<Rigidbody2D>().velocity.x > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.y > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.x < -0.1f ||
            GetComponent<Rigidbody2D>().velocity.y < -0.1f
            )
        {
            animator.Play("playerWalk");
        }
        else
        {
            animator.Play("playerIdle");
        }
    }

    void restartLevel()
    {
        restartLevelUi.SetActive(false);

        startEvent(4);
    }

    IEnumerator restartRestartCounter()
    {
        yield return new WaitForSeconds(1.5f);
        restartLevelUi.SetActive(false);
        restartButtonCounter = 0;
    }

    void startEvent(int eventNumber)
    {
        eventOn = true;
        eventFrameCounter1 = 0;
        eventFrameCounter2 = 0;
        eventFrameCounter3 = 0;
        eventFrameCounter4 = 0;
        eventFrameCounter5 = 0;

        eventTempBool1 = false;
        eventTempBool2 = false;
        eventTempBool3 = false;
        eventTempBool4 = false;
        eventTempBool5 = false;

        eventToProceed = eventNumber;
    }

    void stopEvent()
    {
        eventOn = false;
        eventFrameCounter1 = 0;
        eventFrameCounter2 = 0;
        eventFrameCounter3 = 0;
        eventFrameCounter4 = 0;
        eventFrameCounter5 = 0;

        eventTempBool1 = false;
        eventTempBool2 = false;
        eventTempBool3 = false;
        eventTempBool4 = false;
        eventTempBool5 = false;

        eventToProceed = 0;
    }

    void proceedEvent(int eventNumber)
    {
        switch (eventNumber)
        {
            // Init value, do nothing.
            case 0:
                break;

            // Level 1 loaded, fade out of black, wait 1.5s and start event 2.
            case 1:
                fadeBlack.SetActive(true);
                Color c = fadeToBlack.color;
                c.a -= 0.02f;
                fadeToBlack.color = c;

                if (c.a <= 0)
                {
                    c.a = 0;
                    eventTempBool1 = true;
                }

                if (eventTempBool1)
                {
                    eventFrameCounter1 += 1;
                }

                if (eventFrameCounter1 > 90)
                {
                    startEvent(2);
                }
                break;

            // Level 1 Starting sequence: Focus,  talk, talk, talk, out focus, end.
            case 2:
                if (!eventTempBool1)
                {
                    eventFrameCounter1 += 2;
                    if (eventFrameCounter1 > 200)
                    {
                        eventFrameCounter1 = 200;
                        eventTempBool1 = true;
                        eventTempBool2 = true;
                    }
                    RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, eventFrameCounter1);
                    rt = lowerFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, eventFrameCounter1);
                }

                if (eventTempBool2)
                {
                    eventFrameCounter2 += 1;
                    //print(eventFrameCounter2);
                    if (eventFrameCounter2 < 60 && eventFrameCounter2 < 120)
                    {
                        
                        VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, "Urghh..  Hrrghh..", VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.NORMAL);
                    }
                    else if (eventFrameCounter2 > 160 && eventFrameCounter2 < 300)
                    {
                        VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, "Hmm..?", VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.NORMAL);
                    }
                    else if (eventFrameCounter2 > 340 && eventFrameCounter2 < 800)
                    {
                        VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, "Alice?! What have I done!\nI'm so sorry!  What's going on,\n why she is dead and why am eating her?", VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.NORMAL);
                    }
                    else if (eventFrameCounter2 > 840 && eventFrameCounter2 < 1040)
                    {
                        VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, "I need to get out of here.", VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.NORMAL);
                    }
                    else if (eventFrameCounter2 > 1040)
                    {
                        eventTempBool2 = false;
                        mainCamera.SendMessage("useStrictCamera", false);
                        mainCamera.SendMessage("setTarget", eventCameraTarget1);
                        eventTempBool3 = true;
                    }
                }

                if (eventTempBool3)
                {

                    eventFrameCounter3 += 1;
                    if (eventFrameCounter3 < 120)
                    {
                        VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, "Maybe I can open the door with button.", VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.NORMAL);
                    }
                    else if (eventFrameCounter3 > 120 && eventFrameCounter3 < 240)
                    {
                        mainCamera.SendMessage("setTarget", this.gameObject);
                    }
                    else if (eventFrameCounter3 > 240)
                    {
                        eventTempBool3 = false;
                        eventTempBool4 = true;
                        eventFrameCounter4 = 200;
                    }
                }

                if (eventTempBool4)
                {
                    eventFrameCounter4 -= 2;
                    if (eventFrameCounter4 < 0)
                    {
                        mainCamera.SendMessage("useStrictCamera", true);
                        eventFrameCounter4 = 0;
                        eventTempBool4 = false;
                        stopEvent();

                    }

                    RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, eventFrameCounter4);
                    rt = lowerFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, eventFrameCounter4);

                }
                break;

            // Level 1 Open first zombiedoor: Play alarm, start spin lights, focus in,
            // camera on door, set target for zombies, camera on player, out focus, end.
            case 3:
                if (!eventTempBool1)
                {
                    GameObject audioPlayer = GameObject.FindGameObjectWithTag("audioPlayer");
                    audioPlayer.SendMessage("changeSong", audioPlayerScript.allMusics.ALARM);
                    eventAlarmLight1.SendMessage("toggleTurnedOn");
                    eventAlarmLight2.SendMessage("toggleTurnedOn");
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }

                if (eventTempBool2)
                {
                    eventFrameCounter1 += 4;
                    if (eventFrameCounter1 < 200)
                    {
                        RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter1);
                        rt = lowerFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter1);
                    }
                    else if (eventFrameCounter1 > 250)
                    {
                        eventTempBool2 = false;
                        eventTempBool3 = true;
                        mainCamera.SendMessage("useStrictCamera", false);
                        mainCamera.SendMessage("setTarget", eventCameraTarget2);
                        eventZombieSpawn1.SendMessage("activateSpawn");
                    }
                }

                if (eventTempBool3)
                {
                    eventFrameCounter2++;

                    if (eventFrameCounter2 > 240 && eventFrameCounter2 < 280)
                    {
                        mainCamera.SendMessage("setTarget", this.gameObject);
                    }
                    else if (eventFrameCounter2 > 280)
                    {
                        eventTempBool3 = false;
                        eventTempBool4 = true;
                        eventFrameCounter3 = 200;
                    }
                }

                if (eventTempBool4)
                {
                    eventFrameCounter3 -= 4;
                    if (eventFrameCounter3 > 0)
                    {
                        RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                        rt = lowerFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                    }
                    if (eventFrameCounter3 < -50)
                    {
                        GameObject audioPlayer = GameObject.FindGameObjectWithTag("audioPlayer");
                        audioPlayer.SendMessage("changeSong", audioPlayerScript.allMusics.HAUNTING);
                        eventAlarmLight1.SendMessage("toggleTurnedOn");
                        eventAlarmLight2.SendMessage("toggleTurnedOn");

                        mainCamera.SendMessage("useStrictCamera", true);
                        stopEvent();
                    }
                }
               
                break;

            // Restart current level. Fade, load level.
            case 4:
                Color col = fadeToBlack.color;

                if (!eventTempBool1)
                {
                    fadeBlack.SetActive(true);
                    col.a = 0.0f;
                    fadeToBlack.color = col;
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }

                if (eventTempBool2)
                {
                    col.a += 0.06f;
                    if (col.a > 1.0f)
                    {
                        col.a = 1.0f;
                        eventTempBool2 = false;
                        eventTempBool3 = true;
                    }
                    fadeToBlack.color = col;
                }

                if (eventTempBool3)
                {
                    if (currentLevel == 1)
                    {
                        SceneManager.LoadScene("level1_island");
                    }
                    else if (currentLevel == 2)
                    {
                        SceneManager.LoadScene("level2");
                    }
                    else if (currentLevel == 3)
                    {
                        SceneManager.LoadScene("level3");
                    }
                }
                break;

                // Level 1 first time using speaker: speaker Scrtz, focus in, move camera to speaker,
                // speaker talk, guard questionmark, guard start patrol, speaker talk, move camera to player,
                // focus out, end.
            case 5:
                if (!eventTempBool1) {
                    // Button already connected to speaker
                    //eventLoudspeaker1.SendMessage("toggleSpeaker");
                    playOnce(allSfx.RADIOKOHINA);
                    eventTempBool1 = true;
                }

                if (!eventTempBool2)
                {
                    eventFrameCounter1++;
                    if (eventFrameCounter1 > 45)
                    {
                        eventFrameCounter2 += 4;
                        if (eventFrameCounter2 < 200)
                        {
                            RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                            rt.sizeDelta = new Vector2(1, eventFrameCounter2);
                            rt = lowerFocusGo.GetComponent<RectTransform>();
                            rt.sizeDelta = new Vector2(1, eventFrameCounter2);
                        }
                        else if (eventFrameCounter2 > 250)
                        {
                            eventTempBool2 = true;
                            eventTempBool3 = true;
                            mainCamera.SendMessage("useStrictCamera", false);
                            mainCamera.SendMessage("setTarget", eventCameraTarget3);
                            eventHuman1.SendMessage("togglePatrol");
                            eventHuman2.SendMessage("togglePatrol");

                        }
                    }
                }

                if (eventTempBool3)
                {
                    eventFrameCounter4++;

                    if (eventFrameCounter4 > 240 && eventFrameCounter4 < 280)
                    {
                        mainCamera.SendMessage("setTarget", this.gameObject);
                    }
                    else if (eventFrameCounter4 > 280)
                    {
                        eventTempBool3 = false;
                        eventTempBool4 = true;
                        eventFrameCounter3 = 200;
                    }
                }


                if (eventTempBool4)
                {
                    eventFrameCounter3 -= 4;
                    if (eventFrameCounter3 > 0)
                    {
                        RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                        rt = lowerFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                    }
                    if (eventFrameCounter3 < -50)
                    {
                        soundPlayer.Stop();
                        mainCamera.SendMessage("useStrictCamera", true);
                        stopEvent();
                    }
                }
                break;

            // Level 1 end: Stop camera, add +y force to player, wait, fade in black, load level 2.
            case 6:

                Color coll = fadeToBlack.color;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);

                if (!eventTempBool1)
                {
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    mainCamera.SendMessage("stopFollowing");
                    fadeBlack.SetActive(true);
                    col.a = 0.0f;
                    fadeToBlack.color = coll;
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }


                if (eventTempBool2)
                {
                    eventFrameCounter1++;

                    if (eventFrameCounter1 > 60)
                    {
                        coll.a += 0.06f;
                        if (coll.a > 1.0f)
                        {
                            coll.a = 1.0f;
                            eventTempBool2 = false;
                            eventTempBool3 = true;
                        }
                        fadeToBlack.color = coll;
                    }

                    if (eventTempBool3)
                    {
                        SceneManager.LoadScene("level2");
                        stopEvent();
                    }
                }

                break;

            // Level 2 start: move player, fade out, focus in, stop player, move camera to player, focus out, end.
            case 7:

                Color coll2 = fadeToBlack.color;

                if (!eventTempBool1)
                {
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    mainCamera.SendMessage("stopFollowing");
                    fadeBlack.SetActive(true);
                    col.a = 1.0f;
                    fadeToBlack.color = coll2;
                    RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, 200);
                    rt = lowerFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, 200);
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }


                if (eventTempBool2)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                    eventFrameCounter1++;

                    if (eventFrameCounter1 > 90)
                    {
                        coll2.a -= 0.06f;
                        if (coll2.a < 0.0f)
                        {
                            coll2.a = 0.0f;
                            eventTempBool2 = false;
                            eventTempBool3 = true;
                        }
                        fadeToBlack.color = coll2;
                    }
                }
                if (eventTempBool3)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                    eventFrameCounter2++;

                    if (eventFrameCounter2 > 60)
                    {
                        mainCamera.SendMessage("useStrictCamera", true);
                        mainCamera.SendMessage("setTarget", transform.gameObject);
                        mainCamera.SendMessage("startFollowing");
                        eventTempBool3 = false;
                        eventTempBool4 = true;
                        eventFrameCounter3 = 200;
                    }
                }
                if (eventTempBool4)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                    eventFrameCounter3 -= 4;
                    if (eventFrameCounter3 > 0)
                    {
                        RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                        rt = lowerFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                    }
                    if (eventFrameCounter3 < -50)
                    {
                        mainCamera.SendMessage("useStrictCamera", true);
                        eventGo1.SetActive(true);
                        stopEvent();
                    }
                }

                break;
            // Level 2 end: Stop camera, add -z force to player, wait, fade in black, load level 3.
            case 8:

                Color coll3 = fadeToBlack.color;
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);

                if (!eventTempBool1)
                {
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    mainCamera.SendMessage("stopFollowing");
                    fadeBlack.SetActive(true);
                    coll3.a = 0.0f;
                    fadeToBlack.color = coll3;
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }


                if (eventTempBool2)
                {
                    eventFrameCounter1++;

                    if (eventFrameCounter1 > 60)
                    {
                        coll3.a += 0.06f;
                        if (coll3.a > 1.0f)
                        {
                            coll3.a = 1.0f;
                            eventTempBool2 = false;
                            eventTempBool3 = true;
                        }
                        fadeToBlack.color = coll3;
                    }

                    if (eventTempBool3)
                    {
                        SceneManager.LoadScene("level3");
                        stopEvent();
                    }
                }

                break;
            // Level 3 start: Stop camera, add -z force to player, wait, fade in black, load level 3.
            case 9:

                Color coll4 = fadeToBlack.color;

                if (!eventTempBool1)
                {
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    mainCamera.SendMessage("stopFollowing");
                    fadeBlack.SetActive(true);
                    coll4.a = 1.0f;
                    fadeToBlack.color = coll4;
                    RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, 200);
                    rt = lowerFocusGo.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(1, 200);
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }


                if (eventTempBool2)
                {
                    //GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
                    eventFrameCounter1++;

                    if (eventFrameCounter1 > 90)
                    {
                        coll4.a -= 0.06f;
                        if (coll4.a < 0.0f)
                        {
                            coll4.a = 0.0f;
                            eventTempBool2 = false;
                            eventTempBool3 = true;
                        }
                        fadeToBlack.color = coll4;
                    }
                }
                if (eventTempBool3)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
                    eventFrameCounter2++;

                    if (eventFrameCounter2 > 60)
                    {
                        mainCamera.SendMessage("useStrictCamera", true);
                        mainCamera.SendMessage("setTarget", transform.gameObject);
                        mainCamera.SendMessage("startFollowing");
                        eventTempBool3 = false;
                        eventTempBool4 = true;
                        eventFrameCounter3 = 200;
                    }
                }
                if (eventTempBool4)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
                    eventFrameCounter3 -= 4;
                    if (eventFrameCounter3 > 0)
                    {
                        RectTransform rt = upperFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                        rt = lowerFocusGo.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(1, eventFrameCounter3);
                    }
                    if (eventFrameCounter3 < -50)
                    {
                        mainCamera.SendMessage("useStrictCamera", true);
                        eventGo1.SetActive(true);
                        stopEvent();
                    }
                }
                break;

            // Level 3 end: Stop camera, add -z force to player, wait, fade in black, load level 3.
            case 10:

                Color coss = fadeToBlack.color;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);

                if (!eventTempBool1)
                {
                    itemUi.SetActive(false);
                    invOn = false;
                    Time.timeScale = 1.0F;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    mainCamera.SendMessage("stopFollowing");
                    fadeBlack.SetActive(true);
                    coss.a = 0.0f;
                    fadeToBlack.color = coss;
                    eventTempBool1 = true;
                    eventTempBool2 = true;
                }


                if (eventTempBool2)
                {
                    eventFrameCounter1++;

                    if (eventFrameCounter1 > 60)
                    {
                        coss.a += 0.06f;
                        if (coss.a > 1.0f)
                        {
                            coll3.a = 1.0f;
                            eventTempBool2 = false;
                            eventTempBool3 = true;
                        }
                        fadeToBlack.color = coss;
                    }

                    if (eventTempBool3)
                    {
                        SceneManager.LoadScene("credits");
                        stopEvent();
                    }
                }

                break;
            default:
                break;
        }
    }
    private void updateGameStatus()
    {
        if (!checkIfAlive())
        {
            startEvent(4);
        }
    }

    bool checkIfAlive()
    {
        if (health <= 0)
        {
            return false;
        }
        return true;
    }

    public void distanceToGameobject(int distance)
    {
        distanceToGo = distance;
    }
}