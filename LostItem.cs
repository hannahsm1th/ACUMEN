using UnityEngine;

public class LostItem : Item
{
    public string owner;

    public void Awake() {
        itemType = ItemType.LostItem;
    }
    public override void Use() {
        base.Use();
    }
}
