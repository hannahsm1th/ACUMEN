using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DataPanel : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDescription;
    NPC npc;

    public void Show (NPC input)
	{
        npc = input;
		icon.sprite = npc.icon;
		icon.enabled = true;
		npcName.text = npc.name;
        npcDescription.text = npc.npcDescription;
	}
	public void Hide ()
	{
		npc = null;

		icon.sprite = null;
		icon.enabled = false;
		npcName.text = "";
        npcDescription.text = "";
	}
}
