using UnityEngine;

public class LightGate : MonoBehaviour
{
    [SerializeField] GameObject Beam;
    BoxCollider2D _bc;
    LightGateButton _gateButton;

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
        switch (_gateButton.Active)
        {
            case true:
                _bc.enabled = false;
                Beam.SetActive(false);
                break;

            case false:
                _bc.enabled = true;
                Beam.SetActive(true);
                break;
        }
    }
}
