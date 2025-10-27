using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int row;
    public int col;
    public string color;
    private GameObject bubbleObject;
    private int countCollisionWithBorder = 0;

     public void SetBubblePosition(int x, int y)
    {
        this.row = x; 
        this.col = y;   
    }
   
    public Tuple<int,int> GetPosition()
    {
        Tuple<int,int> index = new Tuple<int,int>(row, col);
        return index;
    }

    public void SetColor(string color)
    {
        this.color = color;
    }

    public string GetColor()
    {
        return this.color;
    }

    public void setCount()
    {
        this.countCollisionWithBorder += 1;
    }

    public int getCount()
    {
        return this.countCollisionWithBorder;
    }
    public void SetBubbleGameObject(GameObject bubble)
    {
        bubbleObject = bubble;
    }

    public GameObject GetBubbleGameObject()
    {
        return this.bubbleObject;
    }
}
