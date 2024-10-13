using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyManager : MonoBehaviour
{
    [SerializeField] int NumberOfFireflies;
    [SerializeField] float Radius;
    [SerializeField] float Speed;

    private GameObject _fireflyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _fireflyPrefab = Resources.Load<GameObject>("_PREFABS/Firefly");
        for (int i = 0; i < NumberOfFireflies; i++)
        {
            Firefly firefly = Instantiate(_fireflyPrefab, transform).GetComponent<Firefly>();
            firefly.Speed = Speed;
            firefly.Radius = Radius;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
