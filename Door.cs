using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Environment
{
    public Panel panel;

    public override void Use()
    {
        base.Use();

        panel.Use();
    }
}
