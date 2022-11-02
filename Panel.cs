using UnityEngine;
public enum PanelType
{
    Key,
    Puzzle
}

public class Panel : Environment
{
    public AudioClip successSound;

    public PanelType type;
    public override void Use()
    {
        base.Use();

        if (this.type == PanelType.Key) {
            OpenWithKey();
        }
        if (this.type == PanelType.Puzzle) {
            OpenWithPuzzle();
        }
    }

    public void OpenWithKey()
    {
        Key key = GameObject.FindWithTag("DoorKey").GetComponent<Key>();

        Debug.Log("Trying to open door.");
        if ( Inventory.instance.Have(key))
        {
            SoundManager.instance.PlaySingle(successSound);
            Debug.Log("Opening... ");
            GameObject.FindWithTag("Door").SetActive(false);
            GameObject.FindWithTag("Panel").SetActive(false);
            GameObject.FindWithTag("DoorKey").SetActive(false);
            key.RemoveFromInventory();

        }
        else
        {
            GameUI.instance.ShowMessagePanel("You need the key for this door!");
        }
    }

    public void OpenWithPuzzle() {
        GameUI.instance.ShowPuzzleControls();
    }
}