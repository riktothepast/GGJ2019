using UnityEngine;

public class ActivateNewMemory : MonoBehaviour
{
    [SerializeField]
    InteractiveObject interactiveObject;

    MementoManager manager;
    private void Start()
    {
        MementoManager manager = FindObjectOfType<MementoManager>();
    }

    public void Activate()
    {
        manager.ActivateObject(interactiveObject.GetIDToUnlock());
    }
}
