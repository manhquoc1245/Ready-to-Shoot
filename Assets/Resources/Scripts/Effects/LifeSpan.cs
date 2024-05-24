using Unity.Netcode;
using UnityEngine;

public class LifeSpan : NetworkBehaviour {
    public float lifeSpan;
    private int iMulti;
    void Start ()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
    }
    void Update() {
        if (iMulti == 1)
        {
            if (!IsServer) 
                return;
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
                NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        }
        else
            Destroy(gameObject, lifeSpan);
    }
}
