using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionPanel : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    Item item;

    public void Show (Item input)
	{
        item = input;
		icon.sprite = item.icon;
		icon.enabled = true;
		itemName.text = item.name;
        itemDescription.text = item.itemDescription;
	}
	public void Hide ()
	{
		item = null;

		icon.sprite = null;
		icon.enabled = false;
		itemName.text = "";
        itemDescription.text = "";
	}
}