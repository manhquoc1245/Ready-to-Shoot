using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;

public class NetworkCameraController : NetworkBehaviour
{
    private CinemachineVirtualCamera playerCamera;

    void Update()
    {
        if(!IsOwner)
        {
            return;
        }

        AttachCamera();
    }

    void AttachCamera()
    {

            playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
            playerCamera.Follow = transform;
            playerCamera.LookAt = transform;

    }
}

