using UnityEngine;
using Ink.Runtime;

public class NPCInteract : Interactable
{
    public NPC npc;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public void Awake() {
        this.message += "<i>\nPress 'E' to talk</i>";
    }

    public override void Interact()
    {
        base.Interact();

        Talk();
    }

    void Talk()
    {
        Debug.Log("Talking to " + npc.name);

        if (!Data.instance.Have(npc)) {
            Data.instance.Add(npc);
        }
        int itemLocation = ItemCheck();
        if (itemLocation > -1) {
            npc.haveLostItem = true;
            string haveItemVariable = npc.name.ToLower() + "_haveitem";
            //Update have item
            DialogueManager.instance.SetVariableState(haveItemVariable, true);
        }

        DialogueManager.instance.EnterDialogueMode(inkJSON, npc);
    }

    private int ItemCheck() {
        int check = Inventory.instance.HaveLostItem(npc);
        return check;
    }
}
