using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTile : MonoBehaviour
{
    // Loads the next level according to the Player entering a trigger or by distance
    public Transform player;
    private bool shouldExit;

    private void Update() {
        TriggerCheck();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            shouldExit = true;
        }
    }

    void TriggerCheck() {
        if (shouldExit) {
            GameManager.instance.Win();
        }
    }
}
