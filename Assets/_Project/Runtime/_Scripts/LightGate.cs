using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGate : MonoBehaviour
{
    LightGateButton _gateButton;

    BoxCollider2D _bc;

    private bool _isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        _gateButton = GetComponentInChildren<LightGateButton>();
        _bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gateButton.Active)
        {
            _bc.enabled = false;
        }
        else if (!_gateButton.Active)
        {
            _bc.enabled = true;
        }

    }
}
