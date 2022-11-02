using UnityEngine;

public enum NPCType
{
    Mimie,
    Solstice,
    Neptune,
    Acacia,
    Sunni,
    Caspar,
    Scarlett,
    Tanner,
    Charlie,
    Olive,
    Default
}

public class NPC : MonoBehaviour
{
    new public string name = "New NPC";
    public string displayName = "???";
    public Sprite icon = null;
    public Sprite menuIcon = null;
    public NPCType npcType;
    public bool alreadyMet;
    public bool nameKnown;
    public int trust;
    public bool haveLostItem = false;
    public bool lostItemReturned;

    [TextArea(5,5)]
    public string npcDescription; // The description that will appear in the data file
    [TextArea(5,5)]
    public string dialogueText; // This will appear after the NPC has been met.
    [TextArea(5,5)]
    public string highTrustText; // This text will display if the NPC has high trust with Nyx.


    public void AddToData() {
        Data.instance.Add(this);
    }

    public void UpdateData() {
        Debug.Log("Updating information for " + name);
        // Check if we learned their name
        string knowNameVariable = this.name.ToLower() + "_knowname";
        nameKnown = ((Ink.Runtime.BoolValue) DialogueManager.instance.GetVariableState(knowNameVariable)).value;
        if (nameKnown) {
            Data.instance.UpdateDataFile(this, 0);
        }
        //Check if we completed the meeting
        string metVariable = name.ToLower() + "_met";
        bool met = ((Ink.Runtime.BoolValue) DialogueManager.instance.GetVariableState(metVariable)).value;
        if (met) {
            Data.instance.UpdateDataFile(this, 1);
        }
        // Update trust
        string trustVariable = name.ToLower() + "_trust";
        trust = ((Ink.Runtime.IntValue) DialogueManager.instance.GetVariableState(trustVariable)).value;
        // Update if we returned the item
        string returnedItemVariable = name.ToLower() + "_returneditem";
        lostItemReturned = ((Ink.Runtime.BoolValue) DialogueManager.instance.GetVariableState(returnedItemVariable)).value;
        if (lostItemReturned) {
            Data.instance.UpdateDataFile(this, 2);
            haveLostItem = false;
            int index = Inventory.instance.HaveLostItem(this);
            if (index > -1) {
                Inventory.instance.Remove(Inventory.instance.items[index]);
            }
        }
    }
}