using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legOnPlatform : MonoBehaviour
{
    private Transform oldParent;
    [SerializeField] private bool active;
    private LineRenderer myLine;

    private void Awake()
    {
        oldParent = transform.parent;
        myLine = GetComponent<LineRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("movingPlatform"))
        {
            transform.parent = collision.gameObject.transform;
        }
    }
    public void OldParent()
    {
        transform.parent = oldParent;
    }

    public bool Active
    {
        get => active;
        set
        {
            active = value;
        }
    }

}
