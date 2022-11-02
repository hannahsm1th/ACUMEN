using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    #region Singleton

    public static Inventory instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            items = new List<Item>();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
    # endregion

    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    public int space = 20; // inventory slots

    public bool Add(Item item) {
        if (items.Count >= space)  {
            Debug.Log("Not enough room.");
            return false;
        }
        items.Add(item);
        if (onItemChangeCallback != null) {
            onItemChangeCallback.Invoke();
        }
        return true;
    }
    public bool Have(Item item) {
        int index = items.FindIndex(a => a.name == item.name);
        if (index > -1) {
            return true;
        }
        else {
            return false;
        }
    }
    public int HaveLostItem(NPC npc) {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemType == ItemType.LostItem) {
                LostItem lostItem = (LostItem) items[i];
                if (lostItem.owner == npc.name) {
                    return i;
                }
            }
        }
        return -1;
    }
    public void Remove(Item item) {
        Debug.Log("removing " + item.name);
        int index = items.FindIndex(a => a.name == item.name);
        items.RemoveAt(index);
        if (onItemChangeCallback != null) { onItemChangeCallback.Invoke();}
    }
}