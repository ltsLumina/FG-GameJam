using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGateButton : MonoBehaviour
{

    private bool _status = false;
    [SerializeField] GameObject buttonON;
    [SerializeField] GameObject buttonOFF;

    public bool Active
    {
        get { return _status; }
    }

    SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _status = !_status;
        }
    }


    // Start is called before the first frame updateW
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_status)
        {
            buttonON.SetActive(true);
            buttonOFF.SetActive(false);
        }
        else if (!_status)
        {
            buttonON.SetActive(false);
            buttonOFF.SetActive(true);
        }
    }
}
