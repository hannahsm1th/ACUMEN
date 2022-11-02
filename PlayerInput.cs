using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D playerRB;
    private Animator animator;
    [SerializeField] private float speed = 5;
    public delegate void OnInteracting();
    public OnInteracting onInteractCallBack;
    private bool submitPressed = false;
    public static PlayerInput instance;
    public AudioClip[] moveSounds;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Game UI Manager in the scene.");
		}
		instance = this;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement (InputValue value) {
        if (!GameManager.instance.doingSetup && !DialogueManager.instance.dialogueIsPlaying)
        {
            movement = value.Get<Vector2>();
            if (movement.x != 0 || movement.y != 0) {
                animator.SetFloat("X", movement.x);
                animator.SetFloat("Y", movement.y);

                animator.SetBool("IsWalking", true);
            }
            else {
                animator.SetBool("IsWalking", false);
            }
        }
    }
    private void OnInteract() {
        if (onInteractCallBack != null && !DialogueManager.instance.dialogueIsPlaying) {
            //onInteractCallBack.Invoke();
        }
    }
    private void OnSubmit() {
        submitPressed = true;
    }
    private void FixedUpdate() {
        // Update movement
        playerRB.MovePosition(playerRB.position + movement * speed * Time.fixedDeltaTime);
    }
    public void PlayWalk()
    {
        SoundManager.instance.RandomiseSfx(moveSounds);
    }
    public bool GetSubmitPressed() {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
}