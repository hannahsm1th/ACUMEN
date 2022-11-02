using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // All information we use for creating a dialogue
    [TextArea(2, 10)]
    public string[] sentences;
    public string name;
}
