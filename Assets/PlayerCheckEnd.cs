#region
using DG.Tweening;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public class PlayerCheckEnd : MonoBehaviour
{
    [SerializeField] Animator endAnimator;
    [SerializeField] TransitionAnimator endTransition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<Player>();
            var playerAnim = other.gameObject.GetComponentInChildren<Animator>();
            player.GetComponentInChildren<SpriteRenderer>().flipX = true;
            playerAnim.Play("Idle");

            endAnimator.enabled = true;
            DOVirtual.DelayedCall(7.5f, () => endTransition.Play());

            Debug.Log("the player should win here");
            other.gameObject.GetComponent<Player>().enabled = false;
            other.gameObject.GetComponentInChildren<PlayerInput>().enabled = false;
        }
    }
}
