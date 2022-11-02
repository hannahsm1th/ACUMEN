using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //<------- Variables ------->
    public static GameManager instance { get; private set; }
    [SerializeField] private  GameObject levelImage;
    [SerializeField] private  GameObject winText;
    public bool doingSetup { get; private set; }
    private GameObject player;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private NPC NyxData;
    private bool gameStart;

    //<------- Methods ------->
    private void Awake() {
        // Enforce the singleton pattern
        if (!gameStart) {
            instance = this;
            gameStart = true;
        }
        else {
            Debug.Log("More than one instance of Game Manager found in the scene!");
            // Destroy(gameObject);
        }
    }
    private void StartGame() {
        SoundManager.instance.levelMusic.Play();
        // this prevents the player from moving while the title card is displaying
        doingSetup = true;
        // Blocks out the screen with the level image for the specified delay time
        levelImage.SetActive(true);
        // Sets the player object
        player = GameObject.FindGameObjectWithTag("Player");
        // Raises the black screen
        levelImage.GetComponent<Animator>().SetBool("isUp", true);
        // Hides the UI buttons
        GameUI.instance.HideButtons();
    }
    void Start() {
        // Starts a new game
        StartGame();
        // Hides the player sprite
        player.GetComponent<SpriteRenderer>().enabled = false;
        // Triggers the opening dialogue
        DialogueManager.instance.EnterDialogueMode(inkJSON, null);
        // Adds Nyx's data file to the Data object
        Data.instance.Add(NyxData);
    }
    public void HideLevelImage() {
        // Set doingSetup to false allowing player to move again
        doingSetup = false;
        // Lower the black screen
        levelImage.GetComponent<Animator>().SetBool("isUp", false);
        // Make the player sprite appear
        player.GetComponent<SpriteRenderer>().enabled = true;
        // Set the UI buttons to active
        GameUI.instance.ShowButtons();
    }
    public void UnloadScene(int scene) {
        // Unloads scenes
        StartCoroutine(Unload(scene));
    }
    IEnumerator Unload(int scene) {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }
    public void Win() {
        // Blocks out the screen with the level image for the specified delay time
        levelImage.SetActive(true);
        levelImage.GetComponent<Animator>().SetBool("isUp", true);
        winText.SetActive(true);
        gameStart = true;
        player.GetComponent<SpriteRenderer>().enabled = false;
        //Disable this Game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}