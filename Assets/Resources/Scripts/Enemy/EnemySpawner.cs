using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform[] pos1, pos2, pos3;
    [SerializeField]
    private GameObject enemy1, enemy2, enemy3;
    private int iMulti;
    void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        if (iMulti == 1)
        {
            if (!IsServer)
                return;   
        }
        foreach (Transform p1 in pos1)
        {
            if(iMulti == 0)
                Instantiate(enemy1,p1.position,Quaternion.identity);
            else
                NetworkObjectSpawner.SpawnNewNetworkObject(enemy1,p1.position,Quaternion.identity);
        }

        foreach (Transform p2 in pos2)
        {
            if (iMulti == 0)
                Instantiate(enemy1, p2.position, Quaternion.identity);
            else
                NetworkObjectSpawner.SpawnNewNetworkObject(enemy2, p2.position, Quaternion.identity);
        }

        foreach (Transform p3 in pos3)
        {
            if (iMulti == 0)
                Instantiate(enemy1, p3.position, Quaternion.identity);
            else
                NetworkObjectSpawner.SpawnNewNetworkObject(enemy3, p3.position, Quaternion.identity);
        }

    }

}
