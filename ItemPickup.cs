using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public void Awake() {
        this.message += "\n<i>Press 'E' to pick up<i>";
    }
    public override void Interact()  {
        base.Interact();

        PickUp();
    }
    void PickUp() {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp) {
            Destroy(gameObject);
            //gameObject.SetActive(false);

            if (item.itemType == ItemType.LostItem)
            {
                LostItem thisItem = (LostItem) item;
                string haveItemVariable = thisItem.name.ToLower() + "_haveitem";
                DialogueManager.instance.SetVariableState(haveItemVariable, true);
            }
        }
    }
}