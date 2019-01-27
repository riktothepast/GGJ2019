using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    //[SerializeField]
    public int id;
    //[SerializeField]
    public string tname;
    //[SerializeField]
    public string display_name;
    //[SerializeField]
    public string description;
    //[SerializeField]
    public status currentStatus;
    //[SerializeField]
    public int idToUnlock;
    //[SerializeField]
    public string[] hints;

    public UnityEvent actions;
    public UnityEvent onActive;
    public UnityEvent onDeactivate;
    bool canBeInteracted;

    public enum status {
        locked,
        unlocked
    }

    private void OnMouseDown()
    {
        Interact();
    }

    private void Start()
    {
        MementoManager manager = FindObjectOfType<MementoManager>();
        if (manager)
        {
            manager.AddInteractableObject(this);
        }
    }

    public void Interact()
    {
        if (canBeInteracted)
        {
            actions.Invoke();
        }
    }

    public void SetStatus(status status)
    {
        currentStatus = status;
    }

    public int GetID()
    {
        return id;
    }

    public int GetIDToUnlock()
    {
        return idToUnlock;
    }

    public string GetDescription()
    {
        return description;
    }

    public string GetName()
    {
        return name;
    }

    public void Activate(bool value)
    {
        canBeInteracted = value;
        if (canBeInteracted)
        {
            onActive.Invoke();
        } else {
            onDeactivate.Invoke();
        }
    }

    public bool IsActive()
    {
        return canBeInteracted;
    }
}
