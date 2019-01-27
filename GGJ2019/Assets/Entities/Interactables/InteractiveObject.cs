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

    [SerializeField]
    public GameObject availableObjects;

    public UnityEvent actions;
    public UnityEvent onActive;
    public UnityEvent onDeactivate;
    bool canBeInteracted;
    AudioManager audioManager;
    MementoManager mementoManager;

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
        audioManager = FindObjectOfType<AudioManager>();
        mementoManager = FindObjectOfType<MementoManager>();
        if (mementoManager)
        {
            mementoManager.AddInteractableObject(this);
        }
    }

    public void Interact()
    {
        if (canBeInteracted)
        {
            actions.Invoke();
            audioManager.SetAudioState(AudioManager.audioStates.discoveringObject);
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
        bool previousState = canBeInteracted;
        canBeInteracted = value;
        if (canBeInteracted)
        {
            onActive.Invoke();
            audioManager.SetAudioState(AudioManager.audioStates.nearObject);
        } else {
            onDeactivate.Invoke();
            audioManager.SetAudioState(AudioManager.audioStates.walking);
        }
    }

    public bool IsActive()
    {
        return canBeInteracted;
    }

    public void AssignObject(int id)
    {
        availableObjects.transform.GetChild(id - 1).gameObject.SetActive(true);
        Debug.Log("AssignObject("+id+")" +
            availableObjects.transform.GetChild(id - 1).gameObject.name
            );
    }
}
