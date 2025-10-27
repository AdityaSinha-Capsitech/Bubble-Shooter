using System.Collections;
using UnityEngine;

public class MoveBubbleDown : MonoBehaviour
{

    public float fallSpeed = 1f; 

    void Update()
    {
        StartCoroutine(StartFalling());
        
    }


    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(2f); 
        while (true)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
