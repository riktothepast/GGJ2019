using System.Collections.Generic;
using UnityEngine;

public class MementoManager : MonoBehaviour
{
    List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();
    void Start()
    {
        
    }

    public void AddInteractableObject(InteractiveObject interactable)
    {
        interactiveObjects.Add(interactable);
    }

    public void ActivateObject(int idToUnlock)
    {
        foreach (InteractiveObject interactive in interactiveObjects)
        {
            if (interactive.GetID().Equals(idToUnlock))
            {
                interactive.SetStatus(InteractiveObject.status.unlocked);
            }
        }
    }
}
