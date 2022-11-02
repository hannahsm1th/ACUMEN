using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public List<NPC> npcs;
    #region Singleton
    public static Data instance;
    void Awake() {
        //Check if instance already exists, to enforce the singleton pattern
        if (instance == null) {
            instance = this;
            npcs = new List<NPC>();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
    # endregion
    public delegate void OnDataChange();
    public OnDataChange OnDataChangeCallback;
    private int space = 11; // data slots
    public bool Add(NPC npc) {
        if (npcs.Count >= space) {
            Debug.Log("Not enough room in data.");
            return false;
        }
        npcs.Add(npc);
        if (OnDataChangeCallback != null) {
            Debug.Log("Invoking data change update of UI");
            OnDataChangeCallback.Invoke();
        }
        return true;
    }
    public bool Have(NPC npc) {
        int index = npcs.FindIndex(a => a.name == npc.name);
        if (index > -1) {
            return true;
        }
        else {
            return false;
        }
    }

    public void UpdateDataFile(NPC npc, int updateCode)  {
        int index = npcs.FindIndex(a => a.name == npc.name);
        if (updateCode == 1) {
            npc.displayName = npc.name;
        }
        if (updateCode == 1) {
            if( npc.dialogueText != "") {
                npc.npcDescription = npc.dialogueText;
                npc.dialogueText = "";
            }
        }
        if (updateCode == 2) {
            if (npc.highTrustText != "") {
                npc.npcDescription += "\n";
                npc.npcDescription += npc.highTrustText;
                npc.highTrustText = "";
            }
        }
        if (OnDataChangeCallback != null) {
            OnDataChangeCallback.Invoke();
        }
    }
}
