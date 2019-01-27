
using TMPro;
using UnityEngine;

public class MessageCanvas : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI message;
    [SerializeField]
    string baseMessage;
    //I have found {itemName}...\n  It reminds me of: {itemDescription}

    public void SetMessageData(string name, string description)
    {
        baseMessage = baseMessage.Replace("{itemName}", name);
        baseMessage = baseMessage.Replace("{itemDescription}", description);
        message.text = baseMessage;
    }

    public void DestroyMessage()
    {
        Destroy(gameObject);
    }
}
