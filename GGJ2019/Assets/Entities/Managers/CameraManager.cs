using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineBrain cinemachineBrain;
    List<CinemachineVirtualCamera> vrCameraList = new List<CinemachineVirtualCamera>();

    public void SetLiveCamera(CinemachineVirtualCamera vrCamera)
    {
        foreach (CinemachineVirtualCamera vr in vrCameraList)
        {
            vr.Priority = 0;
        }
        vrCamera.Priority = 1;
    }

    public void AddCameraToTrackList(CinemachineVirtualCamera vrCamera)
    {
        vrCameraList.Add(vrCamera);
    }
}
