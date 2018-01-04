using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fightingHumanAi : MonoBehaviour
{

    public int health = 10;
    public int STUNTIME = 4;
    public int punchForce = 3;
    public int hitDamage = 1;

    private Animator animator;            // Reference to the player's animator component.


    private Vector2 target;
    private Vector2 closestZombie;
    public float speed;
    public bool stayPut = false;
    public bool patrol = false;
    public bool patrolDone = false;
    public bool loopPatrolRoute = false;

    public GameObject patrolTarget1;
    public GameObject patrolTarget2;
    public GameObject patrolTarget3;
    public GameObject patrolTarget4;
    public GameObject patrolTarget5;
    public GameObject patrolTarget6;
    public GameObject patrolTarget7;
    public GameObject patrolTarget8;
    public GameObject patrolTarget9;
    public GameObject patrolTarget10;

    private GameObject currentPatrolTarget;

    private Vector2 targetDirection;
    private float xDiff;
    private float yDiff;
    private float distance;

    public enum States { IDLE, SELECTING_RAND, WALKING_TO_TARGET, RUNNING_AWAY, STUN, SHOOT, PATROL };
    private int wall;

    public States currentState = States.IDLE;

    private GameObject[] zombies;
    public GameObject player;

    private GameObject targetHuman;

    private Transform bulletSpawn;
    private GameObject shotgun;

    private bool cooledDown = true;
    public int clipSize = 3;
    private int bulletsLeft;
    private int reloadingCounter;

    public float bulletSpeed = 45f;
    public Rigidbody2D bullet;

    private Transform exlamation;
    private bool markStarted = false;
    private bool markEnded = true;
    private int markFrameCounter = 0;

    public bool dancing = false;
    private int danceCounter = 0;


    void Start()
    {
        bulletsLeft = clipSize;
        reloadingCounter = 0;
        // Enable Raycasting towards "wall" layer, which is eight.
        wall = 1 << 8;
        bulletSpawn = transform.Find("shotgun/bulletSpawn");
        exlamation = transform.Find("exlamation");

        zombies = GameObject.FindGameObjectsWithTag("zombie");
        player = GameObject.FindGameObjectWithTag("player");
        animator = GetComponent<Animator>();

        currentPatrolTarget = patrolTarget1;

        if (patrol)
        {
            currentState = States.PATROL;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (dancing)
        {
            danceCounter++;
            if (danceCounter < 40)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
            }
            else if (danceCounter < 80)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
            }
            else if (danceCounter < 120)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed);
            }
            else if (danceCounter < 160)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed);
            }
            else if (danceCounter < 200)
            {
                danceCounter = 0;
            }
        }
        else
        {
            switch (currentState)
            {
                case States.IDLE:
                    if (patrol && !patrolDone)
                    {
                        currentState = States.PATROL;
                    }
                    else
                    {
                        StartCoroutine(Idle());
                    }
                    break;

                case States.SELECTING_RAND:
                    break;


                case States.WALKING_TO_TARGET:
                    if (stayPut)
                    {
                        xDiff = target.x - transform.position.x;
                        yDiff = target.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            //GetComponent<Rigidbody2D>().AddForce(targetDirection.normalized * speed);
                            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                        }
                    }
                    else
                    {
                        xDiff = target.x - transform.position.x;
                        yDiff = target.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            GetComponent<Rigidbody2D>().AddForce(targetDirection.normalized * speed);
                            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                        }
                    }


                    break;


                case States.RUNNING_AWAY:

                    distance = Vector2.Distance(target, transform.position);
                    if (distance < 4)
                    {
                        currentState = States.SHOOT;
                    }
                    else if (distance < 6)
                    {
                        if (markEnded)
                        {
                            markEnded = false;
                            markStarted = true;
                        }

                        xDiff = target.x - transform.position.x;
                        yDiff = target.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            GetComponent<Rigidbody2D>().AddForce(-targetDirection.normalized * speed);
                            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                        }
                        else
                        {
                            currentState = States.IDLE;
                        }
                    }
                    else
                    {
                        currentState = States.IDLE;
                    }

                    break;
                case States.STUN:
                    StartCoroutine(Stun());
                    break;

                case States.SHOOT:
                    if (cooledDown)
                    {
                        if (bulletsLeft > 0) { ShootBullets(); }
                        else { StartCoroutine(reloading()); }
                        cooledDown = false;
                        StartCoroutine(cooling());
                    }
                    else
                    {
                        closestZombie = FindClosestZombie().transform.position;
                        if (Vector2.Distance(closestZombie, transform.position) < 5)
                        {
                            xDiff = closestZombie.x - transform.position.x;
                            yDiff = closestZombie.y - transform.position.y;

                            targetDirection = new Vector2(xDiff, yDiff);
                            Debug.DrawRay(transform.position, targetDirection, Color.red);
                            if (!Physics2D.Raycast(transform.position, targetDirection, 3, wall))
                            {
                                target = closestZombie;
                            }
                        }
                        xDiff = target.x - transform.position.x;
                        yDiff = target.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        distance = Vector2.Distance(target, transform.position);

                        if (distance > 7)
                        {
                            currentState = States.IDLE;
                        }
                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            GetComponent<Rigidbody2D>().AddForce(-targetDirection.normalized * speed / 2);
                            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                        }
                        else
                        {
                            currentState = States.IDLE;
                        }
                    }

                    currentState = States.IDLE;
                    break;
                case States.PATROL:

                    target = currentPatrolTarget.transform.position;
                    xDiff = target.x - transform.position.x;
                    yDiff = target.y - transform.position.y;

                    targetDirection = new Vector2(xDiff, yDiff);
                    Debug.DrawRay(transform.position, targetDirection, Color.red);

                    if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                    {
                        GetComponent<Rigidbody2D>().AddForce(targetDirection.normalized * speed);
                        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                    }

                    break;
                default:
                    break;
            }

            if (currentState != States.SHOOT)
            {
                closestZombie = FindClosestZombie().transform.position;
                if (Vector2.Distance(closestZombie, transform.position) < 5)
                {
                    xDiff = closestZombie.x - transform.position.x;
                    yDiff = closestZombie.y - transform.position.y;

                    targetDirection = new Vector2(xDiff, yDiff);
                    Debug.DrawRay(transform.position, targetDirection, Color.red);
                    if (!Physics2D.Raycast(transform.position, targetDirection, 3, wall))
                    {
                        target = closestZombie;
                        currentState = States.RUNNING_AWAY;

                    }
                }
            }

            if (Random.Range(0, 1000) > 998)
            {
                if (!patrol)
                {
                    currentState = States.IDLE;
                }
            }


            if (markStarted)
            {
                exlamationMark();
            }
        }

        updateZombieTags();
        updateAnimations();
    }

    IEnumerator Idle()
    {
        currentState = States.SELECTING_RAND;
        yield return new WaitForSeconds(Random.Range(0, 2));

        if (currentState == States.SELECTING_RAND)
        {
            target.x = transform.position.x + Random.Range(-5, 5);
            target.y = transform.position.y + Random.Range(-5, 5);
            currentState = States.WALKING_TO_TARGET;

            yield return new WaitForSeconds(2);
            if (currentState == States.WALKING_TO_TARGET)
            {
                currentState = States.IDLE;
            }
        }
    }
    void applyDamage(int amount)
    {
        health -= amount;
        if (!checkIfAlive())
        {
            die();
        }
        stunned();

    }
    bool checkIfAlive()
    {
        if (health <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    void die()
    {
        gameObject.SetActive(false);
    }

    void attackTarget()
    {
        targetHuman.SendMessage("applyDamage", hitDamage);
        Rigidbody2D rbHuman = targetHuman.GetComponent<Rigidbody2D>();
        xDiff = target.x - transform.position.x;
        yDiff = target.y - transform.position.y;
        targetDirection = new Vector2(xDiff, yDiff);

        rbHuman.AddForce(targetDirection.normalized * punchForce);
        if (!targetHuman.activeInHierarchy)
        {
            currentState = States.IDLE;
        }
    }

    void stunned()
    {
        currentState = States.STUN;
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(STUNTIME);
        currentState = States.IDLE;
    }
    GameObject FindClosestZombie()
    {
        GameObject closest;
        closest = null;
        Vector3 diff;
        float curDistance;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in zombies)
        {
            diff = go.transform.position - position;
            curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        position = transform.position;
        diff = player.transform.position - position;
        curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
            closest = player;
        }
        
        return closest;
    }
    IEnumerator cooling()
    {
        yield return new WaitForSeconds(1);
        cooledDown = true;
    }
    void ShootBullets()
    {
        Vector3 basicSpawn = bulletSpawn.position;
        Vector3 spawn1 = basicSpawn;
        float[] exes = { -10, -5, 0, 1, 2, 5, 10 };
        float[] yais = { -30, -20, -25, -22, -21, -29, -28 };
        for (int i = 0; i < 7; i++)
        {
            Rigidbody2D bul = Instantiate(bullet, bulletSpawn.position, transform.rotation) as Rigidbody2D;
            Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
            rb.velocity = transform.TransformDirection(exes[i], yais[i], 0);
        }
        float dist = Vector3.Distance(transform.position, player.transform.position);
        player.SendMessage("distanceToGameobject", dist);
        player.SendMessage("playOnce", playerMovement.allSfx.SHOTGUN);
        bulletsLeft--;
    }
    IEnumerator reloading()
    {
        reloadingCounter++;
        yield return new WaitForSeconds(3);
        if (reloadingCounter > 0)
        {
            bulletsLeft = clipSize;
            reloadingCounter = 0;
        }
    }

    public void targetReached()
    {
        if (currentPatrolTarget == patrolTarget1)
        {
            if (patrolTarget2 != null)
            {
                currentPatrolTarget = patrolTarget2;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget2)
        {
            if (patrolTarget3 != null)
            {
                currentPatrolTarget = patrolTarget3;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget3)
        {
            if (patrolTarget4 != null)
            {
                currentPatrolTarget = patrolTarget4;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget4)
        {
            if (patrolTarget5 != null)
            {
                currentPatrolTarget = patrolTarget5;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget5)
        {
            if (patrolTarget6 != null)
            {
                currentPatrolTarget = patrolTarget6;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget6)
        {
            if (patrolTarget7 != null)
            {
                currentPatrolTarget = patrolTarget7;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget7)
        {
            if (patrolTarget8 != null)
            {
                currentPatrolTarget = patrolTarget8;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget8)
        {
            if (patrolTarget9 != null)
            {
                currentPatrolTarget = patrolTarget9;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget9)
        {
            if (patrolTarget10 != null)
            {
                currentPatrolTarget = patrolTarget10;
            }
            else
            {
                if (loopPatrolRoute)
                {
                    currentPatrolTarget = patrolTarget1;
                }
                else
                {
                    stayPut = true;
                    patrolDone = true;
                    currentState = States.WALKING_TO_TARGET;
                }
            }
        }
        else if (currentPatrolTarget == patrolTarget10)
        {
            
            if (loopPatrolRoute)
            {
                currentPatrolTarget = patrolTarget1;
            }
            else
            {
                stayPut = true;
                patrolDone = true;
                currentState = States.IDLE;
            }
        }
    }

    public void addTarget(GameObject target)
    {
        patrolTarget1 = target;
        patrol = true;
        currentPatrolTarget = patrolTarget1;
    }

    void updateZombieTags()
    {
        zombies = GameObject.FindGameObjectsWithTag("zombie");
    }

    public void togglePatrol()
    {
        if (patrol)
        {
            patrol = false;
        }
        else
        {
            patrol = true;
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
            animator.Play("guardWalk");
        }
        else
        {
            animator.Play("guardIdle");
        }
    }

    void exlamationMark()
    {
        exlamation.gameObject.SetActive(true);

        markFrameCounter++;

        if (markFrameCounter < 25)
        {
            exlamation.transform.localScale.Set(markFrameCounter / 100 * 4, markFrameCounter / 100 * 4, 1.0f);
        }


        if (markFrameCounter > 120)
        {
            markStarted = false;
            exlamation.gameObject.SetActive(false);
            markEnded = true;
        }
    }

    public void startDancing()
    {
        dancing = true;
    }
}
