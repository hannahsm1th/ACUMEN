using UnityEngine;

public class Key : Item
{
    public void Awake()
    {
        itemType = ItemType.Key;
    }
    public override void Use()
    {
        base.Use();
    }
}