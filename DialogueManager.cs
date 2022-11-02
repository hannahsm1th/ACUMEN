using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
//using Ink.UnityIntegration;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance {get; private set; }

    [Header("Parameters")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    //[Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator layoutAnimator;
    [SerializeField] private GameObject continueButton;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private DialogueVariables dialogueVariables;
    private NPC dialoguePartner;

    void Awake() {
        //Check if instance already exists, to enforce the singleton pattern
        if (instance != null) {
            Debug.Log("More than one instance of Dialogue Manager found in the scene!");
            // Destroy(gameObject);
            instance = this;
        }
        instance = this;
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }
    private void Start() {
        // Sets the dialogue to be not playing
        dialogueIsPlaying = false;
        // Gets the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update() {
        // Exits if a dialogue isn't playing
        if (!dialogueIsPlaying) {
            return;
        }
        // continue to the next line in the dialogue
        if (canContinueToNextLine
            && currentStory.currentChoices.Count == 0
            && PlayerInput.instance.GetSubmitPressed()
        ){
            ContinueStory();
        }
    }
    public void EnterDialogueMode(TextAsset inkJSON, NPC conversationPartner) {
        if (conversationPartner != null) {
            dialoguePartner = conversationPartner;
        }
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        // Raises the dialogue panel
        animator.SetTrigger("Open");
        // Hide the inventory and data panels if open
        GameUI.instance.HideInventory();
        GameUI.instance.HideData();
        GameUI.instance.HideButtons();

        dialogueVariables.StartListening(currentStory);
        nameText.text = "???";
        portraitAnimator.Play("Default");
        layoutAnimator.Play("left");

        ContinueStory();
    }
    private IEnumerator ExitDialogueMode()
    {
        Debug.Log("Exiting dialogue with " + dialoguePartner);
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        dialogueIsPlaying = false;
        animator.SetTrigger("Close");
        GameUI.instance.ShowButtons();
        dialogueText.text = "";
        GameManager.instance.HideLevelImage();
        EventSystem.current.SetSelectedGameObject(null);
        if (dialoguePartner != null) {
            dialoguePartner.UpdateData();
        }
        dialoguePartner = null;
    }
    public void ContinueButton() {
        if (canContinueToNextLine && currentStory.currentChoices.Count == 0) {
            ContinueStory();
        }
    }
    private void ContinueStory() {
        if (currentStory.canContinue) {
            // set text for the current dialogue line
            if (displayLineCoroutine != null) {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            // handle tags
            HandleTags(currentStory.currentTags);
        }
        else {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private IEnumerator DisplayLine(string line) {
        // sets the the line and but sets the visible characters to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        // Hide items while text is typing
        continueButton.SetActive(false);
        HideChoices();
        // Manages the flow of dialogue
        canContinueToNextLine = false;
        // Detects if there is a rich text tag
        bool isAddingRichTextTag = false;
        // display each letter one at a time
        foreach (char letter in line.ToCharArray()) {
            // if the submit button is pressed, finish up displaying the line right away
            if (PlayerInput.instance.GetSubmitPressed()) {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }
            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') {
                    isAddingRichTextTag = false;
                }
            }
            // if not rich text, add the next letter and wait a small time
            else {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // actions to take after the entire line has finished displaying
        if (currentStory.currentChoices.Count > 0) {
            DisplayChoices();
            canContinueToNextLine = true;
        }
        else if (currentStory.currentChoices.Count == 0)
        {
            continueButton.SetActive(true); // show continue icon again
            EventSystem.current.SetSelectedGameObject(continueButton);
            canContinueToNextLine = true;
        }
    }
    private void HideChoices() {
        foreach (GameObject choiceButton in choices) {
            choiceButton.SetActive(false);
        }
    }
    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags) {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            // handle the tag
            switch (tagKey) {
                case SPEAKER_TAG:
                    nameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        // check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length) {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialise the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices) {
            // Set choice to be active
            choices[index].gameObject.SetActive(true);
            // Set the text
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
        // Select the first choice
        StartCoroutine(SelectFirstChoice());
    }
    private IEnumerator SelectFirstChoice() {
        // Eventsystem requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine) {
            currentStory.ChooseChoiceIndex(choiceIndex);
            PlayerInput.instance.GetSubmitPressed();
            ContinueStory();
        }
    }
    public Ink.Runtime.Object GetVariableState(string variableName) {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null) {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
    public void SetVariableState(string variableName, bool variableValue) {
        dialogueVariables.StartListening(dialogueVariables.globalVariablesStory);
        if (dialogueVariables.variables.ContainsKey(variableName)) {
            Debug.Log("Setting value for " + variableName + " as " + variableValue);
            dialogueVariables.globalVariablesStory.variablesState[variableName.ToString()] = variableValue;
        }
        dialogueVariables.StopListening(dialogueVariables.globalVariablesStory);
    }

    // This method will get called anytime the application exits.
    // Depending on your game, you may want to save variable state in other places.
    public void OnApplicationQuit() {
        //dialogueVariables.SaveVariables();
    }
}
