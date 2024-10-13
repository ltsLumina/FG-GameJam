#region
using UnityEngine;
using UnityEngine.Rendering.Universal;
#endregion

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] GameObject SpiderBody;
    [SerializeField] GameObject KillLight;
    float _killDistance;

    float _lightFOV;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        var light = KillLight.GetComponent<Light2D>();

        _lightFOV = light.pointLightInnerAngle;
        _killDistance = (light.pointLightInnerRadius + light.pointLightOuterRadius) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = _player.transform.position - transform.position;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, toPlayer);

        if (ray.collider != null)
        {
            bool hasLineOfSight = ray.collider.CompareTag("Player") && IsPlayerInView();

            if (hasLineOfSight)
            {
                _player.Death();
                Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.green);
            }
            else { Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.red); }
        }
    }

    bool IsPlayerInView()
    {
        Vector3 toPLayer = _player.transform.position - transform.position;
        if (toPLayer.magnitude > _killDistance) return false;

        var angleToPlayer = (-Vector3.SignedAngle(toPLayer, new Vector3(1, 0, 0), new Vector3(0, 0, 1)) + 360) % 360;
        float spiderLookDir = transform.rotation.eulerAngles.z;

        float lowerViewBound = spiderLookDir - _lightFOV / 2;
        float upperViewBound = spiderLookDir + _lightFOV / 2;

        if (lowerViewBound < angleToPlayer && angleToPlayer < upperViewBound) return true;
        return false;
    }
}
