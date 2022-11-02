using UnityEngine;

public enum EnvironmentType
{
    Panel,
    Door
}
public class Environment : MonoBehaviour
{
    new public string name = "New Environment";
    public EnvironmentType environmentType;

    public virtual void Use()
    {
        Debug.Log("Using " + name + " of type " + environmentType);
    }
}