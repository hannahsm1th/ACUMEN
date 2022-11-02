using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    //public Button removeButton;
    Item item;
	public GameObject panelObject;
	public DescriptionPanel panel;

    public void AddItem (Item newItem)
	{
		item = newItem;

		icon.sprite = item.icon;
		icon.enabled = true;
		//removeButton.interactable = true;
	}

	// Clear the slot
	public void ClearSlot ()
	{
		item = null;

		icon.sprite = null;
		icon.enabled = false;
		//removeButton.interactable = false;
	}

	// Called when the remove button is pressed
	// public void OnRemoveButton ()
	// {
	// 	Inventory.instance.Remove(item);
	// }

	// Called when the item is pressed
	public void UseItem ()
	{
		if (item != null)
		{
			item.Use();
		}
	}

	public void ShowDescription()
	{
		if (item != null)
		{
			panel.Show(item);
			panelObject.SetActive(true);
		}
	}
	public void HideDescription()
	{
		if (item != null)
		{
			panel.Hide();
			panelObject.SetActive(false);
		}
	}
}
