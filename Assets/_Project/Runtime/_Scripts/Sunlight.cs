#region
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Sunlight : MonoBehaviour
{
    // Change this if you dont want the player to die on collision with the sunlight
    const bool debugToggle = false;

    [SerializeField] GameObject path;

    float _movementSpeed = 1.0f;
    int _nextPosIndex;

    private List<Vector2> _pathPoints = new List<Vector2>();
    Player _player;

    private int _prevPosIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in path.transform)
        {
            Vector2 newPathPoint = new Vector2(0, 0);
            newPathPoint.x = childTransform.position.x;
            newPathPoint.y = childTransform.position.y;
            _pathPoints.Add(newPathPoint);
        }

        transform.position = _pathPoints[0];
        _prevPosIndex = 0;
        _nextPosIndex = 1;

        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goal = new Vector3(_pathPoints[_nextPosIndex].x, _pathPoints[_nextPosIndex].y, 0);
        Vector3 toGoal = goal - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, goal, _movementSpeed * Time.deltaTime);

        if (toGoal.magnitude <= 0.02f)
        {
            if (_nextPosIndex < _pathPoints.Count - 1)
            {
                _prevPosIndex = _nextPosIndex;
                _nextPosIndex++;
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, _player.transform.position - transform.position);

        if (ray.collider != null)
        {
            bool haslineOfSight = ray.collider.CompareTag("Player");

            if (haslineOfSight)
            {
                Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.green);

                if (debugToggle)
                {
                    Debug.Log("Player has entered the sunlight. \nRespawning player at spawn point.");
                    _player.Death();
                }
            }
            else { Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.red); }
        }
    }
}
