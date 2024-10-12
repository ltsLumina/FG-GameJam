#region
using System.Collections;
using TransitionsPlus;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class TutorialGoalTrigger : MonoBehaviour
{
    [SerializeField] TransitionAnimator goalTransition;

    void Start() => goalTransition = GetComponentInChildren<TransitionAnimator>();

    void OnEnable()
    {
#if UNITY_EDITOR

        // Move this component to the top of the component list
        Component[] components = gameObject.GetComponents(typeof(Component));

        for (int index = 0; index < components.Length - 1; index++) { ComponentUtility.MoveComponentUp(this); }
#endif
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            goalTransition.Play();
            StartCoroutine(Hide());
        }

        return;

        IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.25f);
            other.gameObject.SetActive(false);
        }
    }

    void OnValidate()
    {
        goalTransition = GetComponentInChildren<TransitionAnimator>();
        goalTransition.sceneNameToLoad = (SceneManager.GetActiveScene().buildIndex + 1).ToString();

        var rb = GetComponent<Rigidbody2D>();
        var bc = GetComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Static;
        bc.isTrigger = true;

        name = $"Goal: {SceneManagerExtended.ActiveSceneName}";
    }
}
