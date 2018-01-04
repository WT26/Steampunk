using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicZombieAi : MonoBehaviour {

    private Vector2 target;
    public float speed;

    private Vector2 targetDirection;
    private float xDiff;
    private float yDiff;
    private float distance;

    public enum States { IDLE, SELECTING_RAND, WALKING_TO_TARGET, ATTACKING };
    private int wall;

    public States currentState = States.IDLE;

    private GameObject[] humans;

    void Start()
    {
        // Enable Raycasting towards "wall" layer, which is eight.
        wall = 1 << 8;

        humans = GameObject.FindGameObjectsWithTag("human");

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


            case States.ATTACKING:

                distance = Vector2.Distance(target, transform.position);

                if (distance < 7)
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


            default:
                break;
        }

  
        
        foreach (GameObject human in humans)
        {
            distance = Vector2.Distance(human.transform.position, transform.position);
            if (distance < 5)
            {
                xDiff = human.transform.position.x - transform.position.x;
                yDiff = human.transform.position.y - transform.position.y;

                targetDirection = new Vector2(xDiff, yDiff);
                Debug.DrawRay(transform.position, targetDirection, Color.red);

                if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
                {
                    currentState = States.ATTACKING;
                    target = human.transform.position;
                }
            }
        }

        if (Random.Range(0, 1000) > 998)
        {
            currentState = States.IDLE;
        }

    }

    IEnumerator Idle()
    {
        currentState = States.SELECTING_RAND;
        yield return new WaitForSeconds(Random.Range(0,2));
        
        if(currentState == States.SELECTING_RAND)
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
}
