using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class BubbleSpawner : MonoBehaviour
{
    public static BubbleSpawner instance;
    public List<GameObject> bubblesprefabs = new List<GameObject>();
    public GameObject gameOver;
    public Button button;
    public GameObject winText;
    public static FireCannon fired;
    public static ScoreManager scoreInstance;
    public GameObject startQuit;
    public GameObject line;

    public int row = 20; // y- coordinate
    public int col = 9; // x- coordinate
    float xSpacing = 0.5f;
    float ySpacing = 0.42f;
    public int count = 0;
    public bool isGameOver;
    public bool initialScore;
    public bool winGame;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InstantiateGrid();
        InvokeRepeating("RearrangeGrid", 30f, 30f);
        winGame = false;
    }

    void Update()
    {

        GameObject[] allBubbles = GameObject.FindGameObjectsWithTag("Bubbles");

        if (allBubbles.Length <= 1 && isGameOver != true)
        {
            winGame = true;
            startQuit.SetActive(true);
            winText.SetActive(true);
            GameController.gameInstance.SetActivePauseButton(false);
            FireCannon.fireInstance.DestroySpawnBubble();
            line.SetActive(false);
        }
    }

    void InstantiateGrid()
    {
        row = 30;
        col = 9;  

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int index = UnityEngine.Random.Range(0, bubblesprefabs.Count);
                Vector2 position;
                position = GetPosition(i, j);
                GameObject g = Instantiate(bubblesprefabs[index], position, Quaternion.identity);
                setPrefabsInGrid(g, i, j);        
            }
        }

    }
   public void setPrefabsInGrid(GameObject g, int i = 0,int j = 0)
    {
       
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        Bubble bubble = g.GetComponent<Bubble>();

        if (bubble != null)
        {
            Color color = g.GetComponent<SpriteRenderer>().color;
            bubble.SetBubblePosition(i, j);
            bubble.SetColor(ColorUtility.ToHtmlStringRGBA(color));
            bubble.SetBubbleGameObject(g);
        }
    }

    public void RearrangeGrid()
    {
        ValidBubbles();
    }

    public void ValidBubbles(bool flag = true)
    {
        GameObject[] allBubbles = GameObject.FindGameObjectsWithTag("Bubbles");
        int cnt = 0;
        foreach (var bubble in allBubbles)
        {
            if (bubble == null) continue;

            if (bubble.transform.position.y < -3.68f)
            {
                cnt++;
                continue;
            }

            if (flag)
            {
                Vector2 pos = bubble.transform.position;
                pos.y -= 0.5f;
                bubble.transform.position = pos;
            }

        } 

        if (cnt > 1)
        {
            GameOver();
            return;
        }
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        isGameOver = true;
        DestroyAllBubbles();
        startQuit.SetActive(true);
        GameController.gameInstance.SetActivePauseButton(false);
        line.SetActive(false);
    }

    void DestroyAllBubbles()
    {
        foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("Bubbles"))
        {
            Destroy(bubble);
        }
    }

    public Vector2 GetPosition(int i, int j)
    {
        Vector2 position;
        if (i % 2 == 0)
        {
            position = new Vector2(j * xSpacing - 1.84f, -i * ySpacing + 14f);
        }
        else
        {
            position = new Vector2(j * xSpacing - 2.1f, -i * ySpacing + 14f );
        }
        return position;
    }
    public void RestartGame()
    {
        ScoreManager.scoreInstance.InitialScore();
        FireCannon.fireInstance.DestroySpawnBubble();
        InstantiateGrid();
        isGameOver = false; 
        winGame = false; 
        Debug.Log("restart called");
        gameOver.SetActive(false);
        winText.SetActive(false);
        startQuit.SetActive(false);
        GameController.gameInstance.SetActivePauseButton(true);
        line.SetActive(true);
    }

  

}
