using UnityEngine;

public class DetectNearObjects : MonoBehaviour
{
    public LayerMask layerDetected;
    public float distance = 3;

    void Update()
    {
        DoDetect(transform.position, distance);
    }


    void DoDetect(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerDetected);
        Debug.Log("Detectados: " + hitColliders.Length);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("--Detectado (layer:" + hitColliders[i].gameObject.layer + "): " + hitColliders[i].name);
            //hitColliders[i].SendMessage("Anunciarse");
            i++;
        }
    }
}
