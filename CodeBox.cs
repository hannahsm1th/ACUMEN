using UnityEngine;

public class CodeBox : Item
{
    public int code;
    private bool wasOpened;
    public Item contents;
    public AudioClip successSound;

    public void Awake() {
        itemType = ItemType.CodeBox;
        wasOpened = false;
        usable = true;
    }
    public override void Use() {
        base.Use();

        if (!wasOpened) {
            GetCode();
        }
    }
    public void GetCode() {
        Debug.Log("Getting Code");
        GameUI.instance.ShowCodeBoxControls();
    }
    public Item GetContents() {
        SoundManager.instance.PlaySingle(successSound);
        wasOpened = true;
        usable = false;
        Debug.Log("Getting contents of code box...");
        itemDescription = "The code box. You opened it already.";
        return contents;
    }
}
