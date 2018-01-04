using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public float speed;

    private Vector2 targetDirection;
    private float xDiff;
    private float yDiff;
    private float distance;

    private int wall;

    void Start () {
        wall = 1 << 8;
	}
	
	void Update () {
        distance = Vector2.Distance(target.position, transform.position);

        if (distance < 10)
        {
            xDiff = target.position.x - transform.position.x;
            yDiff = target.position.y - transform.position.y;

            targetDirection = new Vector2(xDiff, yDiff);
            Debug.DrawRay(transform.position, targetDirection, Color.red);

            if (!Physics2D.Raycast(transform.position, targetDirection, 1, wall))
            {
                GetComponent<Rigidbody2D>().AddForce(targetDirection.normalized * speed);
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            }
        }
    }
}
