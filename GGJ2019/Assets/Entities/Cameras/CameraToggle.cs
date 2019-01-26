using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera vrCamera;
    CameraManager cameraManager;

    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.AddCameraToTrackList(vrCamera);
    }

    private void OnTriggerEnter(Collider other)
    {
        cameraManager.SetLiveCamera(vrCamera);
    }
}
