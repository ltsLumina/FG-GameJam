#region
using UnityEngine;
#endregion

public class FireflyManager : MonoBehaviour
{
    [SerializeField] int numberOfFireflies;
    [SerializeField] float Radius;
    [SerializeField] float Speed;

    GameObject _fireflyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _fireflyPrefab = Resources.Load<GameObject>("_PREFABS/Firefly");

        for (int i = 0; i < numberOfFireflies; i++)
        {
            var firefly = Instantiate(_fireflyPrefab, transform).GetComponent<Firefly>();
            firefly.Speed = Speed;
            firefly.Radius = Radius;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    void OnValidate() { name = $"Fireflies ({numberOfFireflies}x)"; }
}
