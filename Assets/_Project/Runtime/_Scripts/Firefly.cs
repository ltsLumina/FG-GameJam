using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Firefly : MonoBehaviour
{
    [SerializeField] Light2D _light;
    private float _intensity;

    private float _runnningTime = 0;
    public float Speed = 3f;
    public float Radius = 1f;

    private Vector3 _center;
    private float _xPeriod;
    private float _yPeriod;

    private enum State
    {
        FollowPath, Flee, Return 
    }

    [SerializeField] State _currrentState = State.FollowPath;

    Vector3 _fleeLocation;
    Vector3 _returnPos;

    SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _intensity = _light.intensity;

        _center = transform.position;

        _xPeriod = Random.Range(1f, 3f);
        _yPeriod = Random.Range(1f, 3f);

    }

    // Update is called once per frame
    void Update()
    {
        if (_currrentState == State.FollowPath)
        {
            float newX = _center.x + Mathf.Cos(_xPeriod * _runnningTime) * Radius;
            float newY = _center.y + Mathf.Sin(_yPeriod * _runnningTime) * Radius;

            Vector3 newPos = new Vector3(newX, newY, 0);
            transform.position = newPos;

            _runnningTime += Time.deltaTime * Speed;
        }
        else if (_currrentState == State.Flee)
        {
            transform.position = Vector3.MoveTowards(transform.position, _fleeLocation, Speed * 2 * Time.deltaTime);
            if (transform.position == _fleeLocation)
            {
                _currrentState = State.Return;
            }
        }
        else if (_currrentState == State.Return)
        { 
            transform.position = Vector3.MoveTowards(transform.position, _returnPos, Speed/ 2 * Time.deltaTime);
            if (transform.position == _returnPos)
            {
                _currrentState = State.FollowPath;
                _sr.DOFade(1, 1f);
                DOVirtual.Float(_light.intensity, _intensity, 1f, x => _light.intensity = x);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Scatter();
        }
    }

    void Scatter()
    {
        _currrentState = State.Flee;
        Vector3 toCenter = _center - transform.position;
        _fleeLocation = transform.position + Vector3.ClampMagnitude(toCenter * -3, 2);

        float newX = _center.x + Mathf.Cos(_xPeriod * _runnningTime) * Radius;
        float newY = _center.y + Mathf.Sin(_yPeriod * _runnningTime) * Radius;

        _returnPos = new Vector3(newX, newY, 0);

        _sr.DOFade(0, 1f);
        DOVirtual.Float(_light.intensity, 0, 1f, x => _light.intensity = x);
    }
}
