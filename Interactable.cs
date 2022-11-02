using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public float radius = 1f; // range in which the object becomes interactable
    private Transform player; // The player's transform
    public Transform interactionTransform; // Allows us to make objects interactable from only one angle
    bool hasApproached = false; // Stops the update method from continuously running the Interact method when we move up to an Interactable object
    [TextArea(3,3)]
    public string message; // short message that will display when the player approaches the interactable
    float distance;
    public virtual void Interact() {
        // Virtual class to be overridden by interactable items
    }
    public virtual void UpdateMessage() {}

    void Start() {
        // Gets the player's transform and sets it to the correct variable
        this.player = GameObject.FindWithTag("Player").transform;
        // Subscribe to the callbacks
		PlayerInput.instance.onInteractCallBack += DoInteract;
    }

    void Update() {
        distance = Vector3.Distance(player.position, interactionTransform.position);
        if (!hasApproached) {
            if (distance <= radius) {
                Debug.Log("Approaching " + this.name);
                GameUI.instance.ShowMessagePanel(this.message);
                hasApproached = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E)){
            DoInteract();
        }
        UpdateMessage();
        if (hasApproached) {
            if (distance > radius) {
                Debug.Log("Moving away from " + this.name);
                GameUI.instance.HideMessagePanel();
                hasApproached = false;
            }
        }
    }
    void DoInteract() {
        distance = Vector3.Distance(player.position, interactionTransform.position);
        if (distance <= radius) {
            Debug.Log("Interacting with " + this.name);
            GameUI.instance.HideMessagePanel();
            Interact();
        }
    }
    void OnDrawGizmosSelected () {
        // Draws the radius on the level
        if (interactionTransform == null) {
            interactionTransform = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
