using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicHumanAi : MonoBehaviour
{

    public int health = 10;
    public int STUNTIME = 3;

    private Vector2 target;
    private Vector2 closestZombie;
    public bool stayPut = false;

    public float speed;
    private Animator animator;            // Reference to the player's animator component.

    private Vector2 targetDirection;
    private float xDiff;
    private float yDiff;
    private float distance;

    public enum States { IDLE, SELECTING_RAND, WALKING_TO_TARGET, RUNNING_AWAY, STUN };
    private int wall;

    public States currentState = States.IDLE;

    private GameObject[] zombies;
    public GameObject player;

    public bool dancing = false;
    private int danceCounter = 0;

    void Start()
    {
        // Enable Raycasting towards "wall" layer, which is eight.
        wall = 1 << 8;

        zombies = GameObject.FindGameObjectsWithTag("zombie");
        player = GameObject.FindGameObjectWithTag("player");
        animator = GetComponent<Animator>();

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


                case States.RUNNING_AWAY:

                    distance = Vector2.Distance(target, transform.position);

                    if (distance < 6)
                    {
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

                default:
                    break;
            }
        }


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

        if (Random.Range(0, 1000) > 998)
        {
            currentState = States.IDLE;
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


    void stunned()
    {
        currentState = States.STUN;
    }
    IEnumerator Stun()
    {
        yield return new WaitForSeconds(STUNTIME);
        currentState = States.IDLE;
    }
    void updateZombieTags()
    {
        zombies = GameObject.FindGameObjectsWithTag("zombie");
    }

    void updateAnimations()
    {

        if (GetComponent<Rigidbody2D>().velocity.x > 0.7f ||
            GetComponent<Rigidbody2D>().velocity.y > 0.7f ||
            GetComponent<Rigidbody2D>().velocity.x < -0.7f ||
            GetComponent<Rigidbody2D>().velocity.y < -0.7f
            )
        {
            animator.Play("humanRun");
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.y > 0.1f ||
            GetComponent<Rigidbody2D>().velocity.x < -0.1f ||
            GetComponent<Rigidbody2D>().velocity.y < -0.1f
            )
        {
            animator.Play("humanWalk");
        }
        else
        {
            animator.Play("humanIdle");
        }
    }
    public void startDancing()
    {
        dancing = true;
    }

}