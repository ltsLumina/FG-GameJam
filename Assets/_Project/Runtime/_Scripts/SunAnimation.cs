using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnimation : MonoBehaviour
{

    [SerializeField] LayerMask groundLayer;

    [SerializeField] LayerMask legLayer;

    [SerializeField] float legLenght;

    [SerializeField] int legAmount;

    [SerializeField] int rayAmount;

    List<LineRenderer> legs = new List<LineRenderer>();

    [SerializeField] int FOV;

    [SerializeField] float legSpacing;

    List<LineRenderer> unusedLegs = new List<LineRenderer>();

    [SerializeField] LineRenderer sunLeg;

    [SerializeField] Transform legManager;

    Vector3 lastPos;


    private void Start()
    {

        for(int i = 0; i < legAmount; i++)
        {
            legs.Add(Instantiate(sunLeg, legManager));
            FindNewAnchor(legs[i]);
        }

    }

    private void FixedUpdate()
    {

        Vector3 velocity;

        velocity = (transform.position - lastPos).normalized;
        lastPos = transform.position;

        var newAngle = -Vector3.SignedAngle(velocity, new Vector3(1,0,0), Vector3.forward);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));

        for (int i = 0; i < rayAmount; i++)
        {

            float rad = ((FOV / rayAmount * i) - (FOV/2 - newAngle)) * Mathf.Deg2Rad;

            Vector2 newDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, newDir, legLenght, groundLayer);

            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, newDir, Color.green);
                var legCheck = Physics2D.CircleCast(hit.point, legSpacing, Vector2.zero, 0, legLayer);

                if(unusedLegs.Count > 0 && !legCheck)
                {                 
                    unusedLegs[0].gameObject.SetActive(true);
                    unusedLegs[0].gameObject.transform.position = hit.point;
                    unusedLegs.Remove(unusedLegs[0]);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, newDir, Color.red);
            }
        }

        for(int i = 0; i < legAmount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                legs[i].SetPosition(0, transform.position);
                Vector2 spiderBend = new Vector2((legs[i].transform.position.x + transform.position.x)/2, ((legs[i].transform.position.y + transform.position.y)/2)+1);
                legs[i].SetPosition(1, spiderBend);
                legs[i].SetPosition(2, legs[i].transform.position);
            }
        }
    }


    private void FindNewAnchor(LineRenderer aLeg)
    {
        aLeg.gameObject.SetActive(false);
        unusedLegs.Add(aLeg);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("sunLeg"))
        {
            FindNewAnchor(other.GetComponent<LineRenderer>());
        }
        
    }


}
