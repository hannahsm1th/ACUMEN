using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataSlot : MonoBehaviour
{
    public Image icon;
	public TextMeshProUGUI npcName;
    NPC npc;
	public GameObject panelObject;
	public DataPanel panel;

    public void AddData (NPC newNPC)
	{
		npc = newNPC;
		npcName.text = npc.displayName;
		icon.sprite = npc.menuIcon;
		icon.enabled = true;
	}
    // Clear the file
	public void ClearFile ()
	{
		npc = null;

		icon.sprite = null;
		name = null;
		icon.enabled = false;
	}

	public void ShowDescription()
	{
		if (npc != null)
		{
			panel.Show(npc);
			panelObject.SetActive(true);
		}
	}
	public void HideDescription()
	{
		if (npc != null)
		{
			panel.Hide();
			panelObject.SetActive(false);
		}
	}
}
