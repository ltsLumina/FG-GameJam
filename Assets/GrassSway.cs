using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSway : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Sway");
        }
    }
}
