using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSpawnScript : MonoBehaviour {

    public int maxZombiesToSpawn = 0;
    private int spawnedCount = 0;
    public bool keepSpawning = false;
    public int spawnWaitTime = 1;
    public bool active = true;

    public Rigidbody2D zombiePrefab;
    public GameObject initialPatrolTarget;

    private List<Rigidbody2D> zombieList;
    private bool spawnWaitOver = true;

	// Use this for initialization
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        zombieList = new List<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (zombieList.Count < maxZombiesToSpawn)
            {
                if (spawnWaitOver)
                {
                    if (!keepSpawning && spawnedCount >= maxZombiesToSpawn)
                    {
                    }
                    else
                    {
                        spawnZombie();
                    }
                }
            }
            updateZombieList();
        }
	}

    void spawnZombie()
    {
        Vector3 pos = transform.position;
        pos.x += Random.Range(-1, 1);
        pos.y += Random.Range(-1, 1);

        Rigidbody2D zombie = Instantiate(zombiePrefab, pos, transform.rotation) as Rigidbody2D;
        //Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
        //rb.velocity = transform.TransformDirection(exes[i], yais[i], 0);
        if (initialPatrolTarget != null)
        {
            zombie.SendMessage("addTarget", initialPatrolTarget);
        }
        zombie.gameObject.tag = "zombie";
        zombie.transform.parent = gameObject.transform;
        zombieList.Add(zombie);
        spawnWaitOver = false;
        spawnedCount += 1;
        StartCoroutine(spawnWait());
        
    }

    void updateZombieList()
    {
        for (int i = 0; i != zombieList.Count; i++)
        {
            if (zombieList[i] == null)
            {
                zombieList.RemoveAt(i);
            }
        }
    }

    IEnumerator spawnWait()
    {
        yield return new WaitForSeconds(spawnWaitTime);
        spawnWaitOver = true;
    }

    public void activateSpawn()
    {
        active = true;
    }
    public void deactivateSpawn()
    {
        active = false;
    }
}
