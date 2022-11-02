using UnityEngine;

public enum ItemType
{
    Key,
    LostItem,
    CodeBox,
    Default
}

public class Item : MonoBehaviour
{
    new public string name = "New Item";
    public Sprite icon = null; // The sprite that will appear in the inventory
    public ItemType itemType = ItemType.Default;
    public bool usable = false;

    [TextArea(15,20)]
    public string itemDescription; // The description that will appear in the inventory

    public virtual void Use ()
    {
        if (usable)
        {
            Debug.Log("Using " + name + " of type " + itemType);
        }
    }

    public void AddToInventory()
    {
        Inventory.instance.Add(this);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}