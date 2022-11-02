using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* This object updates the inventory UI. */

public class GameUI : MonoBehaviour {

	[SerializeField] private Transform itemsParent;	// The parent object of all the items
	[SerializeField] private Transform filesParent;	// The parent object of all the files
	public GameObject buttons;
	[SerializeField] private GameObject messagePanel;
	[SerializeField] private GameObject codeBoxControls;
	[SerializeField] private GameObject puzzleControls;
	[SerializeField] private GameObject InventoryUI;
	[SerializeField] private GameObject DataUI;
	public static GameUI instance;
	Inventory inventory;	// Our current inventory
	Data data; //Our current datafile
	InventorySlot[] slots;	// Array of all the slots
	DataSlot[] files;

	void Awake() {
		if (instance != null) {
			Debug.LogError("Found more than one Game UI Manager in the scene.");
		}
		instance = this;
	}
	void Start () {
		// Set the data and inventories
		inventory = Inventory.instance;
		data = Data.instance;

		// Subscribe to the callbacks
		inventory.onItemChangeCallback += UpdateInventoryUI;
		data.OnDataChangeCallback += UpdateDataUI;

		// Populate our slots array
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		files = filesParent.GetComponentsInChildren<DataSlot>();

		// Calls an update here to ensure the initial Nyx datafile is viewable
		UpdateDataUI();
	}
	void UpdateInventoryUI () {
        Debug.Log("Updating InventoryUI");
		// Loop through all the slots and updates if there is an item to be added
		for (int i = 0; i < slots.Length; i++) {
			if (i < inventory.items.Count)	{
				slots[i].AddItem(inventory.items[i]);
			} else {
				// Otherwise clear the slot
				slots[i].ClearSlot();
			}
		}
	}
	void UpdateDataUI () {
        Debug.Log("Updating DataUI");
		// Loop through all the slots
		for (int i = 0; i < files.Length; i++) {
			if (i < data.npcs.Count)	// If there is an npc to add
			{
				files[i].AddData(data.npcs[i]);	// Add it
			} else {
				// Otherwise clear the file
				files[i].ClearFile();
			}
		}
	}
    public void ShowHideInventory() {
        InventoryUI.SetActive(!InventoryUI.activeSelf);
		DataUI.SetActive(false);
    }
	public void HideInventory() {
            InventoryUI.SetActive(false);
    }
	public void ShowHideData() {
        DataUI.SetActive(!DataUI.activeSelf);
		InventoryUI.SetActive(false);
    }
	public void HideData() {
        DataUI.SetActive(false);
    }
	public void ShowButtons() {
		buttons.SetActive(true);
	}
	public void HideButtons() {
		buttons.SetActive(false);
	}
	public void ShowMessagePanel(string text) {
		TextMeshProUGUI messageText = messagePanel.GetComponentInChildren<TextMeshProUGUI>();
		messageText.text = text;
		messagePanel.SetActive(true);
	}
	public void HideMessagePanel() {
		messagePanel.SetActive(false);
	}
	public void ShowCodeBoxControls() {
		codeBoxControls.SetActive(true);
	}
	public void HideCodeBoxControls() {
		codeBoxControls.SetActive(false);
	}
	public void ShowPuzzleControls() {
		puzzleControls.SetActive(true);
	}
	public void HidePuzzleControls() {
		puzzleControls.SetActive(false);
	}
}