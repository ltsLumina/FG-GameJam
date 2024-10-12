using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnimation : MonoBehaviour
{

    [SerializeField] LayerMask layer;

    [SerializeField] float legLenght;

    [SerializeField] int legAmount;

    [SerializeField] int rayAmount;

    [SerializeField] List<LineRenderer> legs = new List<LineRenderer>();

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

        Debug.Log(velocity);

        var newAngle = Vector3.SignedAngle(velocity, Vector3.right, Vector3.up);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));

        for (int i = 0; i < rayAmount; i++)
        {

            float rad = 180 / rayAmount * Mathf.Deg2Rad * i;

            Vector2 newDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad) * transform.rotation.z);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, newDir, legLenght, layer);

            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, newDir, Color.green);
                if(unusedLegs.Count > 0)
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
                legs[i].SetPosition(1, legs[i].transform.position);
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
