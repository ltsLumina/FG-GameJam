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
            Vector3 toPLayer = player.transform.position - transform.position;
            var newAngle = -Vector3.SignedAngle(toPLayer, new Vector3(1, 0, 0), Vector3.forward);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle - 90));
        }
        transform.position = spiderBody.transform.position;
    }
}
