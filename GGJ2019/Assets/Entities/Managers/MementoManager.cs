using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MementoManager : MonoBehaviour
{
    [SerializeField] InteractiveObject baseObject;
    [SerializeField] List<ObjectSpawnPoint> spawnPoints;
    [SerializeField] MessageCanvas message;

    List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();
    void Start()
    {
        LoadObjectsInWorld();
    }

    public void AddInteractableObject(InteractiveObject interactable)
    {
        interactiveObjects.Add(interactable);
    }

    public void ActivateObject(InteractiveObject currentObject)
    {
        foreach (InteractiveObject interactive in interactiveObjects)
        {
            if (interactive.GetID().Equals(currentObject.GetIDToUnlock()))
            {
                interactive.SetStatus(InteractiveObject.status.unlocked);
                MessageCanvas newMessage = Instantiate(message);
                Debug.Log(interactive.ToString());
                newMessage.SetMessageData(currentObject.display_name, interactive.hints[Random.Range(0, interactive.hints.Length)]);
                Time.timeScale = 0;
                break;
            }
        }
    }

    void LoadObjectsInWorld()
    {
        //spawnPoints = new List<ObjectSpawnPoint>();
        Debug.Log("LoadObjectsInWorld!");

        // -- load JSON of possible objects data
        string gameDataFileName = "JsonData.json";
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        Debug.Log(filePath);
        JsonData gameData = null;

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            Debug.Log(dataAsJson);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            gameData = JsonUtility.FromJson<JsonData>(dataAsJson);
            Debug.Log("json?");
            Debug.Log(gameData.objectos);

        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }



        // -- get all spawnpoints
        // -- select (spawnpoints.length) random objects and set UnlockIds

        // -- instantiate objects in spawnpoints
        //List<int> allIds
        int ii = 0;
        foreach (ObjectSpawnPoint spoint in spawnPoints)
        {
            Debug.Log("SpawnPoint! " + spoint.id.ToString());
            Debug.Log("data??! " + gameData.objectos.Length);
            spoint.gameObject.SetActive(false);
            InteractiveObject aa = (InteractiveObject)Instantiate(baseObject, spoint.transform.position, baseObject.transform.rotation);

            int jj = 0;
            foreach (JsonDataObject tobj in gameData.objectos)
            {
                Debug.Log("JsonDataObject! " + tobj.id.ToString());
                if (jj.Equals(ii))
                {
                    aa.id = tobj.id;
                    aa.idToUnlock = tobj.idToUnlock;
                    aa.tname = tobj.tname;
                    aa.display_name = tobj.display_name;
                    aa.description = tobj.description;
                    aa.hints = tobj.hints;
                    aa.name = "object_"+tobj.id + "_" + tobj.tname;
                    aa.AssignObject(tobj.id);
                }
                jj++;
            }

            ii++;
        }
    }

}
