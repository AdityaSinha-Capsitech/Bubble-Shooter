using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public List<GameObject> bubblePrefabs;
    public static FireCannon fireInstance;
    public BubbleSpawner bubbleSpawner;
    private GameObject g;
    public GameObject currentBubble;

    public float speed;
    public bool firstFired = false;
    public float starttime = 0.0f;
    public float delaytime = 2.0f;
    public Vector2 firedPos;
    public bool bubbleFired;

    void Awake()
    {
        fireInstance = this;
    }
    private void Start()
    {
        bubbleSpawner = GameObject.Find("Spawner Manager").GetComponent<BubbleSpawner>();
        starttime = 0.0f;
        delaytime = 1.0f;    
        InvokeRepeating("TrySpawnBubble", starttime, delaytime);
        bubbleFired = false;
    }
   
    void Update()
    {
        if(currentBubble != null && BubbleSpawner.instance.isGameOver)
        {
            Destroy(currentBubble);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        firedPos = transform.position;
    }


    void TrySpawnBubble()
    {
        var isGameOver = BubbleSpawner.instance.isGameOver;
        var winGame = BubbleSpawner.instance.winGame;
        if ((currentBubble == null && !isGameOver && !winGame) || (currentBubble != null && currentBubble.transform.position.y > transform.position.y + 1f && !isGameOver && !winGame))
        {
            SpawnBubble();
        }
    }

    void SpawnBubble()
    {   
        int length = bubblePrefabs.Count;
        int index = UnityEngine.Random.Range(0, length);
        g = Instantiate(bubblePrefabs[index], transform.position, Quaternion.identity);
    
        g.tag = "spwanBubble";
        currentBubble = g;
    }

    public void DestroySpawnBubble()
    {
        if (currentBubble)
        {
            Destroy(currentBubble);
        }
    }

    void Shoot()
    {
        if (g == null)
        {
            return;
        }
        firstFired = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("bubble fired" + bubbleFired);
        if(mousePos.y < -4)
        {
            return;
        }

        var last = g;



        bubbleFired = true;
        mousePos.z = 0;
        var direction = ((mousePos - transform.position).normalized);

        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.AddForce(1.5f * speed * direction, ForceMode2D.Impulse);
        bubbleFired = false;

    }

    public bool FirstFiredOrNot()
    {
        return this.firstFired;
    }

    public void SetFirstFired(bool firstFired)
    {
        this.firstFired = firstFired;   
    }
  
}

    