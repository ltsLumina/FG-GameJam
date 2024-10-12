using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private float _movementSpeed = 1.0f;


    [SerializeField] GameObject path;
    private List<Vector2> _pathPoints = new List<Vector2>();

    private int _prevPosIndex;
    private int _nextPosIndex;



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
}
