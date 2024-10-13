#region
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Sunlight : MonoBehaviour
{
    // Change this if you dont want the player to die on collision with the sunlight
    const bool debugToggle = true;

    [SerializeField] GameObject path;
    [SerializeField] List<float> _pathTimes = new ();

    float _movementSpeed = 1.0f;
    int _nextPosIndex;

    private List<Vector2> _pathPoints = new List<Vector2>();
    private List<float> _pathSpeeds = new List<float>();

    Player _player;

    private int _prevPosIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (!path) return;

        _pathPoints.Clear();

        foreach (Transform childTransform in path.transform)
        {
            Vector2 newPathPoint = new Vector2(0, 0);
            newPathPoint.x = childTransform.position.x;
            newPathPoint.y = childTransform.position.y;
            _pathPoints.Add(newPathPoint);
        }

        for (int i = 0; i < _pathTimes.Count; i++)
        {
            Vector2 startPoint = _pathPoints[i];
            Vector2 endPoint = _pathPoints[i + 1];
            float distanceBetween = (endPoint - startPoint).magnitude;
            float speed = distanceBetween / _pathTimes[i];
            _pathSpeeds.Add(speed);
        }

        transform.position = _pathPoints[0];
        _nextPosIndex = 1;

        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!path) return;

        Vector3 goal = new Vector3(_pathPoints[_nextPosIndex].x, _pathPoints[_nextPosIndex].y, 0);
        Vector3 toGoal = goal - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, goal, _pathSpeeds[_nextPosIndex - 1] * Time.deltaTime);

        if (toGoal.magnitude <= 0.02f)
        {
            //last one
            if (_nextPosIndex < _pathPoints.Count - 2)
            {
                _prevPosIndex = _nextPosIndex;
                _nextPosIndex++;
                Debug.Log(_nextPosIndex);
            }
            else if (_nextPosIndex < _pathPoints.Count - 1) { Debug.Log("Route Done"); }
        }
    }

    void FixedUpdate()
    {
        _player = FindObjectOfType<Player>();
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

    void OnDrawGizmos()
    {
        // draw sphere at path points
        Gizmos.color = Color.yellow;
        foreach (Vector2 pathPoint in _pathPoints) { Gizmos.DrawWireSphere(pathPoint, .45f); }
    }

    void OnValidate()
    {
        if (!path) return;

        _pathPoints.Clear();

        foreach (Transform childTransform in path.transform)
        {
            var newPathPoint = new Vector2(0, 0);
            newPathPoint.x = childTransform.position.x;
            newPathPoint.y = childTransform.position.y;
            _pathPoints.Add(newPathPoint);
        }

        int properTimeCount = _pathPoints.Count - 1;

        if (_pathTimes.Count != properTimeCount)
            if (_pathTimes.Count < properTimeCount)
                _pathTimes.Add(1f);
    }
}
