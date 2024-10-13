using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    [SerializeField] private GameObject spiderBody;

    [SerializeField] private bool lookTowardsPlayer;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (lookTowardsPlayer)
        {
            transform.right = player.transform.position - transform.position;
        }
        transform.position = spiderBody.transform.position;
    }
}
