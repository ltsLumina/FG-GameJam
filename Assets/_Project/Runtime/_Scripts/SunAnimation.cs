using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnimation : MonoBehaviour
{

    [SerializeField] LayerMask layer;

    [SerializeField] float legLenght;

    [SerializeField] int legAmount;

    [SerializeField] List<LineRenderer> legs = new List<LineRenderer>();

    [SerializeField] private LineRenderer sunLeg;


    private void Start()
    {

        for(int i = 0; i < legAmount; i++)
        {
            legs.Add(Instantiate(sunLeg, transform));
        }

    }

    private void FixedUpdate()
    {

        for (int i = 0; i < legAmount; i++)
        {

            float rad = 360 / legAmount * Mathf.Deg2Rad * i;

            Vector2 newDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, newDir, legLenght, layer);

            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, newDir, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, newDir, Color.red);
            }


            for (int j = 0; j < 2; j++)
            {
                legs[i].SetPosition(0, transform.position);
                legs[i].SetPosition(1, legs[i].transform.position);
            }



        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }


}
