using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour
{
    void Start()
    {
        if (IsLocalPlayer)
        {
            // Thêm script đồng bộ hóa Cinemachine chỉ trên máy chơi địa phương
            gameObject.AddComponent<NetworkCameraController>();
        }
    }
}
