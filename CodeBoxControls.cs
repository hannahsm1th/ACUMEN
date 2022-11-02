using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeBoxControls : MonoBehaviour
{
    private CodeBox codeBox;
    public TMP_Dropdown drop1;
    public TMP_Dropdown drop2;
    public TMP_Dropdown drop3;
    public TMP_Dropdown drop4;

    private void OnEnable() {
        int index = Inventory.instance.items.FindIndex(a => a.itemType == ItemType.CodeBox);
        // CHECK IF OPENED HERE??
        codeBox = (CodeBox) (Inventory.instance.items[index]);
    }
    public void OnSubmit()
    {
        Debug.Log("Submitting...");
        string codeString = "";
        GameUI.instance.HideMessagePanel();

        codeString += drop1.value.ToString();
        codeString += drop2.value.ToString();
        codeString += drop3.value.ToString();
        codeString += drop4.value.ToString();

        if (codeString == codeBox.code.ToString()) {
            codeBox.RemoveFromInventory();
            Item contents = codeBox.GetContents();
            codeBox.AddToInventory();
            AddContents(contents);
            GameUI.instance.HideCodeBoxControls();
        }
        else {
            GameUI.instance.ShowMessagePanel("WRONG CODE. TRY AGAIN.");
        }
    }

    public void OnCancel()
	{
		GameUI.instance.HideCodeBoxControls();
        GameUI.instance.HideMessagePanel();
	}
    public void AddContents(Item contents) {
        // Adds contents to the inventory
        contents.AddToInventory();
    }
}
