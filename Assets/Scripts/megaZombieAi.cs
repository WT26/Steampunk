using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class megaZombieAi : MonoBehaviour {

    public int health = 100;
    public int STUNTIME = 1;
    public int punchForce = 7;
    public int hitDamage = 2;

    private Animator animator;            // Reference to the player's animator component.

    private Vector2 target;
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


    public enum States { IDLE, SELECTING_RAND, WALKING_TO_TARGET, ATTACKING, STUN, PATROL };
    private int wall;

    public States currentState = States.IDLE;

    private GameObject[] humans;
    private GameObject player;
    private GameObject targetHuman;

    public Animator punchAnimation;

    private bool playerZombie;

    void Start()
    {
        // Enable Raycasting towards "wall" layer, which is eight.
        wall = 1 << 8;
        punchAnimation = GetComponentInChildren<Animator>();
        humans = GameObject.FindGameObjectsWithTag("human");
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

        switch (currentState)
        {
            case States.IDLE:
                StartCoroutine(Idle());
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


            case States.ATTACKING:

                distance = Vector2.Distance(target, transform.position);
                print(distance);
                if (distance < 2)
                {
                    attackTarget();
                    stunned();

                }
                else if (distance < 20)
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


        if (currentState != States.STUN)
        {

            // Zombies should not attack player in any circumstance. This part will allow that.
            
            player.SendMessage("isZombie", gameObject);
            if (!playerZombie)
            {
                if (player.activeInHierarchy == true)
                {
                    distance = Vector2.Distance(player.transform.position, transform.position);
                    if (distance < 20)
                    {
                        xDiff = player.transform.position.x - transform.position.x;
                        yDiff = player.transform.position.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            currentState = States.ATTACKING;
                            target = player.transform.position;
                            targetHuman = player;
                        }
                    }
                }
            }
            
            /*
            foreach (GameObject human in humans)
            {
                if (human.activeInHierarchy == true)
                {
                    distance = Vector2.Distance(human.transform.position, transform.position);
                    if (distance < 20)
                    {
                        xDiff = human.transform.position.x - transform.position.x;
                        yDiff = human.transform.position.y - transform.position.y;

                        targetDirection = new Vector2(xDiff, yDiff);
                        Debug.DrawRay(transform.position, targetDirection, Color.red);

                        if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                        {
                            currentState = States.ATTACKING;
                            target = human.transform.position;
                            targetHuman = human;
                        }
                    }
                }
            }*/
        }


        if (Random.Range(0, 1000) > 998)
        {
            currentState = States.IDLE;
        }
        updateHumanTags();
        updateAnimations();

    }

    IEnumerator Idle()
    {
        currentState = States.SELECTING_RAND;
        yield return new WaitForSeconds(Random.Range(0, 2));

        if (currentState == States.SELECTING_RAND)
        {
            target.x = transform.position.x + Random.Range(-2, 2);
            target.y = transform.position.y + Random.Range(-2, 2);
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
        print("ATTACKING");
        targetHuman.SendMessage("applyDamage", hitDamage);
        Rigidbody2D rbHuman = targetHuman.GetComponent<Rigidbody2D>();
        xDiff = target.x - transform.position.x;
        yDiff = target.y - transform.position.y;
        targetDirection = new Vector2(xDiff, yDiff);
        //Animation bul = Instantiate(punchAnimation, transform.position, transform.rotation) as Animation;
        //punchAnimation.Play("zombiePunchAnim");
        rbHuman.AddForce(targetDirection.normalized * punchForce);
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


    public void isPlayerZombie(bool answer)
    {
        playerZombie = answer;

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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                    //stayPut = true;
                    patrolDone = true;
                    currentState = States.IDLE;
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
                //stayPut = true;
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
    void updateHumanTags()
    {
        humans = GameObject.FindGameObjectsWithTag("human");
    }

    void updateAnimations()
    {

        if (GetComponent<Rigidbody2D>().velocity.x > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.y > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.x < -0.1f ||
            GetComponent<Rigidbody2D>().velocity.y < -0.1f
            )
        {
            animator.Play("megaWalk");
        }
        else
        {
            animator.Play("megaIdle");
        }
    }
}
