using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleControls : MonoBehaviour
{
    public TMP_Dropdown drop1;
    public TMP_Dropdown drop2;
    public TMP_Dropdown drop3;
    public TMP_Dropdown drop4;
    public TMP_Dropdown drop5;
    private Panel panel;
    [SerializeField] private string answer;
    [SerializeField] private Animator animator;
    public TextMeshProUGUI playsRemaining;
    public int playsRemainingCount;
    public GameObject playButton;

    private void Start() {
        playsRemainingCount = 3;
        playsRemaining.text = playsRemainingCount.ToString();
    }

    private void OnEnable() {
        panel = GameObject.FindWithTag("Panel").GetComponentInChildren<Panel>();
        playsRemaining.text = playsRemainingCount.ToString();
    }
    public void OnSubmit()
    {
        Debug.Log("Submitting puzzle answer...");
        string codeString = "";
        GameUI.instance.HideMessagePanel();

        codeString += drop1.value.ToString();
        codeString += drop2.value.ToString();
        codeString += drop3.value.ToString();
        codeString += drop4.value.ToString();
        codeString += drop5.value.ToString();

        Debug.Log(codeString);
        Debug.Log(answer);

        if (codeString == answer) {
            GameUI.instance.HidePuzzleControls();
            SoundManager.instance.PlaySingle(panel.successSound);
            Debug.Log("Opening... ");
            GameObject.FindWithTag("Door").SetActive(false);
            GameObject.FindWithTag("Panel").SetActive(false);
        }
        else {
            GameUI.instance.ShowMessagePanel("WRONG CODE. TRY AGAIN.");
        }
    }

    public void OnCancel()
	{
		GameUI.instance.HidePuzzleControls();
        GameUI.instance.HideMessagePanel();
	}

    public void OnPlayAgain() {
        if (playsRemainingCount > 1) {
            animator.SetTrigger("Play");
        }
        if (playsRemainingCount == 1) {
            animator.SetTrigger("PlayFinal");
        }
        if (playButton.GetComponentInChildren<TextMeshProUGUI>().text == "Play") {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play again";
        }
        playsRemainingCount -= 1;
        if (playsRemainingCount < 1) {
            playButton.SetActive(false);
            Debug.Log("COUNT DONE");
            Destroy(playButton);
        }
        playsRemaining.text = playsRemainingCount.ToString();
    }
}