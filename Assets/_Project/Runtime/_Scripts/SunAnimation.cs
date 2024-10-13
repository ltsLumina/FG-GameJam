using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnimation : MonoBehaviour
{

    [SerializeField] bool lookingAtPlayer;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] LayerMask legLayer;

    [SerializeField] float legLenght;

    [SerializeField] int legAmount;

    [SerializeField] int rayAmount;

    List<LineRenderer> legs = new List<LineRenderer>();

    [SerializeField] int FOV;

    [SerializeField] float legSpacing;

    List<LineRenderer> unusedLegs = new List<LineRenderer>();

    [Header("The speed of the leg animations")]
    [SerializeField] float bendSpeed;

    [SerializeField] float stretchSpeed;

    private List<legOnPlatform> legOnPlatforms = new List<legOnPlatform>();

    [SerializeField] LineRenderer sunLeg;

    private CircleCollider2D circleCol;

    private List<Vector2> spiderBend = new List<Vector2>();
    private List<Vector2> legPos = new List<Vector2>();

    Vector3 velocity = Vector3.zero;

    Vector3 lastPos;

    private Player player;


    private void Start()
    {
        circleCol = GetComponent<CircleCollider2D>();

        var legManager = new GameObject("legManager");

        for(int i = 0; i < legAmount; i++)
        {
            legs.Add(Instantiate(sunLeg, legManager.transform));
            FindNewAnchor(legs[i]);
            legOnPlatforms.Add(legs[i].GetComponent<legOnPlatform>());
            legPos.Add(new Vector2());
            spiderBend.Add(new Vector2());
        }

        transform.parent = legManager.transform;

        player = FindObjectOfType<Player>();

    }

    private void FixedUpdate()
    {

        float tempFOV = FOV;

        if(transform.position != lastPos)
        {
            velocity = (transform.position - lastPos);
        }

        lastPos = transform.position;

        if (unusedLegs.Count > legAmount - 3)
        {
            tempFOV = 360;
        }

        circleCol.radius = legLenght + 1;

        var newAngle = -Vector3.SignedAngle(velocity.normalized, new Vector3(1,0,0), Vector3.forward);

        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));

        for (int i = 0; i < legAmount; i++)
        {

            RaycastHit2D legHit = Physics2D.Raycast(transform.position, legs[i].transform.position - transform.position, Mathf.Infinity, groundLayer);

            if (legHit.collider != null)
            {
                bool hasLineOfSight = Physics2D.CircleCast(legHit.point, 0.2f, Vector2.zero, 0, legLayer);


                if (!hasLineOfSight && legOnPlatforms[i].Active)
                {
                    FindNewAnchor(legs[i]);
                    //Debug.Log(i);
                    //Debug.DrawLine(transform.position, legHit.point, Color.red);
                }
                else
                {

                    //Debug.DrawLine(transform.position, legHit.point, Color.green);
                }
            }

            var velocityMag = Mathf.Max(velocity.magnitude, 1);

            if (legOnPlatforms[i].Active)
            {
                legPos[i] = Vector2.MoveTowards(legPos[i], legs[i].gameObject.transform.position, stretchSpeed * Time.deltaTime * velocityMag);
                var tempSpiderBend = new Vector2((legs[i].transform.position.x + transform.position.x) / 2, ((legs[i].transform.position.y + transform.position.y) / 2) + 1);
                spiderBend[i] = Vector2.MoveTowards(spiderBend[i], tempSpiderBend, stretchSpeed * Time.deltaTime * velocityMag);
            }
            else
            {
                legPos[i] = Vector2.MoveTowards(legPos[i], transform.position, bendSpeed * Time.deltaTime * velocityMag);
                spiderBend[i] = Vector2.MoveTowards(spiderBend[i], transform.position, bendSpeed * Time.deltaTime * velocityMag);
                legs[i].gameObject.transform.position = transform.position;
            }

            for (int j = 0; j < 2; j++)
            {
                legs[i].SetPosition(0, transform.position);
                legs[i].SetPosition(1, spiderBend[i]);
                legs[i].SetPosition(2, legPos[i]);
            }
        }

        for (int i = 0; i < rayAmount; i++)
        {

            float rad = ((tempFOV / rayAmount * i) - (tempFOV / 2 - newAngle)) * Mathf.Deg2Rad;

            Vector2 newDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, newDir, legLenght, groundLayer);

            //Debug.DrawRay(transform.position, newDir, Color.green, 1);

            if (hit.collider != null)
            {

                var legCheck = Physics2D.CircleCast(hit.point, legSpacing, Vector2.zero, 0, legLayer);

                if(unusedLegs.Count > 0 && !legCheck)
                {                 
                    //unusedLegs[0].gameObject.SetActive(true);
                    unusedLegs[0].gameObject.GetComponent<legOnPlatform>().Active = true;

                    unusedLegs[0].gameObject.transform.position = hit.point;
                    Debug.Log(unusedLegs.Remove(unusedLegs[0]));
                }
            }

            if (lookingAtPlayer)
            {
                transform.right = player.transform.position - transform.position;
            }

        }

        
    }



    private void FindNewAnchor(LineRenderer aLeg)
    {
        //aLeg.gameObject.SetActive(false);
        unusedLegs.Add(aLeg);
        var legPlatform = aLeg.gameObject.GetComponent<legOnPlatform>();
        legPlatform.Active = false;
        legPlatform.OldParent();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("sunLeg") && other.gameObject.GetComponent<legOnPlatform>().Active)
        {
            FindNewAnchor(other.GetComponent<LineRenderer>());
        }
        
    }


}
