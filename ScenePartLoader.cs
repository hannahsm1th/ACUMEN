using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum CheckMethod
{
    Distance,
    Trigger
}
public class ScenePartLoader : MonoBehaviour
{
    // Loads the next level according to the Player entering a trigger or by distance
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;
    private bool isLoaded;
    private bool shouldLoad;


    private void Start() {
        if (SceneManager.sceneCount > 0) {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name) {
                    isLoaded = true;
                }
            }
        }
    }

    private void Update() {
        if (checkMethod == CheckMethod.Distance) {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger) {
            TriggerCheck();
        }
    }

    void DistanceCheck() {
        if (Vector3.Distance(player.position, transform.position) < loadRange) {
            LoadScene();
        }
    }

    void LoadScene() {
        if (!isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    void UnloadScene() {
        if (isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            shouldLoad = false;
        }
    }

    void TriggerCheck() {
        if (shouldLoad) {
            LoadScene();
        }
        else {
            UnloadScene();
        }
    }
}
