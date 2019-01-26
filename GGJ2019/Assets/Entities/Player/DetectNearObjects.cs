using UnityEngine;
using System.Collections.Generic;

public class DetectNearObjects : MonoBehaviour
{
    public LayerMask layerDetected;
    public float distance = 3;
    List<InteractiveObject> interactableObjects = new List<InteractiveObject>();

    void Update()
    {
        DoDetect(transform.position, distance);
    }


    void DoDetect(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerDetected);
        int colliderCount = hitColliders.Length;
        if (colliderCount < interactableObjects.Count)
        {
            List<InteractiveObject> colliderObjects = new List<InteractiveObject>();
            foreach (Collider col in hitColliders)
            {
                colliderObjects.Add(col.GetComponent<InteractiveObject>());
            }
            foreach (InteractiveObject interactable in interactableObjects)
            {
                if (!colliderObjects.Contains(interactable))
                {
                    interactable.Activate(false);
                }
            }
        }
        int i = 0;
        while (i < colliderCount)
        {
            InteractiveObject interactiveObject = hitColliders[i].GetComponent<InteractiveObject>();
            if (!interactableObjects.Contains(interactiveObject))
            {
                interactiveObject.Activate(true);
                interactableObjects.Add(interactiveObject);
            }
            i++;
        }

        for (int interactableIndex = interactableObjects.Count - 1; interactableIndex >= 0; interactableIndex--)
        {
            if (!interactableObjects[interactableIndex].IsActive())
            {
                interactableObjects.Remove(interactableObjects[interactableIndex]);
            }
        }
    }
}
