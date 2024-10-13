using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    private float _runnningTime = 0;
    public float Speed = 3f;
    public float Radius = 1f;

    private Vector3 _center;
    private float _xPeriod;
    private float _yPeriod;

    // Start is called before the first frame update
    void Start()
    {
        _center = transform.position;


        _xPeriod = Random.Range(1f, 3f);
        _yPeriod = Random.Range(1f, 3f);

    }

    // Update is called once per frame
    void Update()
    {

        float newX = _center.x + Mathf.Cos(_xPeriod * _runnningTime) * Radius;
        float newY = _center.y + Mathf.Sin(_yPeriod * _runnningTime) * Radius;

        Vector3 newPos = new Vector3(newX, newY, 0);
        transform.position = newPos;

        _runnningTime += Time.deltaTime * Speed;
    }
}
