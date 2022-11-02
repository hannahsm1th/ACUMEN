using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteract : Interactable
{
    public Environment enviro;
    public override void UpdateMessage()
    {
        base.UpdateMessage();

        if (enviro.environmentType == EnvironmentType.Panel) {
            Panel panel = (Panel) enviro;
            if (panel.type == PanelType.Key) {
                Key key = GameObject.FindWithTag("DoorKey").GetComponent<Key>();

                if ( Inventory.instance.Have(key)) {
                    this.message = "<i>Press 'E' to open the door.</i>";
                }
            }
            if (panel.type == PanelType.Puzzle) {
                this.message = "<i>Press 'E' to try the puzzle.</i>";
            }
        }
    }

    public override void Interact()
    {
        base.Interact();

        enviro.Use();
    }

}