using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legOnPlatform : MonoBehaviour
{
    private Transform oldParent;

    private void Awake()
    {
        oldParent = transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("movingPlatform"))
        {
            transform.parent = collision.gameObject.transform;
        }
    }

    private void OnEnable()
    {
        transform.parent = oldParent;
    }

}
