#region
using UnityEngine;
using UnityEngine.Playables;
#endregion

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline;

    void Start()
    {
        if (!timeline) timeline = GetComponentInParent<PlayableDirector>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && timeline) timeline.Play();
    }

    void OnValidate()
    {
        var rb = GetComponent<Rigidbody2D>();
        var bc = GetComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Static;
        bc.isTrigger = true;

        if (!timeline) return;
        name = $"Trigger: {timeline.gameObject.name}";
        transform.SetParent(timeline.transform);
    }
}
