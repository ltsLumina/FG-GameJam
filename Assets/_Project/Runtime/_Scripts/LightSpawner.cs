using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{

    [SerializeField] GameObject circle;
    private BoxCollider2D boxCollider;

    private int height;
    private int width;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();  
        NewCircles();
    }




    private void NewCircles()
    {

    }

    private Vector2 randomPointInCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
