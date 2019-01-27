using UnityEngine;

public class ActivateNewMemory : MonoBehaviour
{
    [SerializeField]
    InteractiveObject interactiveObject;

    MementoManager manager;
    private void Start()
    {
        manager = FindObjectOfType<MementoManager>();
    }

    public void Activate()
    {
        manager
            .ActivateObject(
            interactiveObject
            );
    }
}
