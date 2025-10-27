using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public static PlayerController playerInstacne;
    private FireCannon newBubble;
    private BubbleSpawner sp;
    public GameObject explosion;
    private GameObject g1;
    private List<GameObject> effectObjects;
    public AudioClip destroySound;

    [SerializeField] private int score;
    private Collider2D[] hits;
    private float connectionRadius;

    void Awake()
    {
        playerInstacne = this;
    }

    void Start()
    {
        connectionRadius = 0.3f;
        effectObjects = new List<GameObject>();

    }

    private void Update()
    {
        if(transform.position.y <= -3.68f && !gameObject.CompareTag("spwanBubble"))
        {
            BubbleSpawner.instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        score = 0;
        newBubble = FireCannon.fireInstance;
        sp = BubbleSpawner.instance;

      

        GameObject player1 = collision.gameObject;
        GameObject player2 = gameObject;

        player2.tag = "Bubbles";

        if (player1.name == "BorderBottom" || player2.name == "BorderBottom")
        {
            Destroy(player2);
            return;
        }
        Rigidbody2D rb1 = player1.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = player2.GetComponent<Rigidbody2D>();

        if(player1.name == "BorderLeft" || player1.name == "BorderRight")
        {
            player2.GetComponent<Bubble>().setCount();
            if(player2.GetComponent<Bubble>().getCount()>= 7)
            {
                Destroy(player2 );
                return;
            }
        }

        if (player1.name != "BorderLeft" && player2.name != "BorderRight" && player2.name != "BorderLeft" && player1.name != "BorderRight")
        {
            rb1.bodyType = RigidbodyType2D.Kinematic;
            rb2.bodyType = RigidbodyType2D.Kinematic;
            rb1.angularVelocity = 0f;
            rb2.angularVelocity = 0f;
            rb1.linearVelocity = Vector2.zero;
            rb2.linearVelocity = Vector2.zero;
            rb1.constraints = RigidbodyConstraints2D.FreezeAll;
            rb2.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (newBubble.FirstFiredOrNot() && player1.name != "BorderLeft" && player2.name != "BorderRight" && player2.name != "BorderLeft" && player1.name != "BorderRight")
        {
            newBubble.SetFirstFired(false);
            sp.setPrefabsInGrid(player2);
            CheckNearbyMatches(collision);
            ScoreManager.scoreInstance.UpdateScoreUI(score);
        }
    }

    public void SetScoreZero()
    {
        score = 0;
    }


    void CheckNearbyMatches(Collision2D collision)
    {
        UnityEngine.Color targetColor = GetComponent<SpriteRenderer>().color;
        UnityEngine.Color otherColor = collision.gameObject.GetComponent<SpriteRenderer>().color;

        Collider2D[] hitsa = Physics2D.OverlapCircleAll(gameObject.transform.position, connectionRadius);
        //Debug.Log("bubble -" + gameObject.name + "length" + hitsa.Length);

        foreach (Collider2D hit in hitsa)
        {
            if (hit.CompareTag("Bubbles"))
            {
                UnityEngine.Color c = hit.GetComponent<SpriteRenderer>().color;
                if (c == targetColor)
                {
                    HandleClusterDestruction(gameObject);
                }
            }
        }
    }

    private void HandleClusterDestruction(GameObject g)
    {
        List<GameObject> cluster = FindConnectedBubbles(g);



        if (cluster.Count >= 3)
        {
            foreach (var b in cluster)
            {
                Debug.Log("score: " + cluster.Count);
                if (b == null) continue;
                var po = b.transform.position;
                g1 = Instantiate(explosion, po, Quaternion.identity);
                StartCoroutine(DestroySomeDelay(b, g1));
            }
            AudioSource.PlayClipAtPoint(destroySound, transform.position, 2.0f);
            score = cluster.Count;
            cluster.Clear();
        }
    }

    private List<GameObject> FindConnectedBubbles(GameObject start)
    {
        List<GameObject> connected = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        UnityEngine.Color targetColor = start.GetComponent<SpriteRenderer>().color;

        queue.Enqueue(start);
        connected.Add(start);


        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            hits = Physics2D.OverlapCircleAll(current.transform.position, connectionRadius);

            //Debug.Log("start " + start.name);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Bubbles") && !connected.Contains(hit.gameObject))
                {
                    UnityEngine.Color c = hit.GetComponent<SpriteRenderer>().color;
                    if (c == targetColor)
                    {
                        connected.Add(hit.gameObject);
                        queue.Enqueue(hit.gameObject);
                    }
                }
            }
        }
        return connected;
    }

    private void DestroyFloatingBubbles()
    {
        List<GameObject> connectedBubbles = new List<GameObject>();
        GameObject[] allBubbles = GameObject.FindGameObjectsWithTag("Bubbles");

        foreach (var bubble in allBubbles)
        {
            if (bubble == null || bubble.transform.position.y < -3.6) continue;

            Collider2D[] hitsa = Physics2D.OverlapCircleAll(bubble.transform.position, connectionRadius);
            bool hasNeighbor = false;

            foreach (var hit in hitsa)
            {
                if (hit.CompareTag("Bubbles") && hit.gameObject != bubble && Bfs(hit.gameObject, connectedBubbles))
                {
                    hasNeighbor = true;
                    break;
                }


            }

            if (!hasNeighbor)
            {
                var po = bubble.transform.position;
                GameObject g1 = Instantiate(explosion, po, Quaternion.identity);
                g1.GetComponent<ParticleSystem>().Play();
                Destroy(bubble);
                effectObjects.Add(g1);
            }

        }
    }


    private bool Bfs(GameObject bubble, List<GameObject> connectedBubbles)
    {

        List<GameObject> connected = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        bool connectedToTop = false;


        queue.Enqueue(bubble);
        connected.Add(bubble);


        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            Collider2D[] hits = Physics2D.OverlapCircleAll(current.transform.position, connectionRadius);

            //Debug.Log("bubble " + bubble.name);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Bubbles") && !connected.Contains(hit.gameObject))
                {

                    //Debug.Log("hit " + hit.name);

                    if (hit.gameObject.transform.position.y >= 5f)
                    {
                        connectedToTop = true;
                        break;
                    }
                    connected.Add(hit.gameObject);
                    queue.Enqueue(hit.gameObject);

                    connectedBubbles.Add(hit.gameObject);

                }
            }
        }

        if (!connectedToTop) DestroyAllBubbles(connected);

        return connectedToTop;
    }

    private void DestroyAllBubbles(List<GameObject> connected)
    {
        foreach (var item in connected)
        {
            if (item)
            {
                var po = item.transform.position;
                GameObject g1 = Instantiate(explosion, po, Quaternion.identity);
                g1.GetComponent<ParticleSystem>().Play();
                Destroy(item);
            }
        }
    }



    IEnumerator DestroySomeDelay(GameObject b, GameObject g)
    {
        yield return new WaitForSeconds(0.00000001f);
        g.GetComponent<ParticleSystem>().Play();
        Destroy(b);
        DestroyFloatingBubbles();
    }

}






