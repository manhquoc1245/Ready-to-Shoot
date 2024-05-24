using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Cinemachine;
using System.Globalization;

public class StructPlayer : MonoBehaviour
{
    [SerializeField]
    private CharacterDataSO[] characterDataSO;
    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private CinemachineVirtualCamera Vcam;
    // Start is called before the first frame update

    int iMulti;
    void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        foreach (CharacterDataSO data in characterDataSO)
        {
            if(data.isSelected && iMulti == 0)
            {
                GameObject playerObj = Instantiate(data.characterPrefab, respawnPoint.position, Quaternion.identity);
                Vcam.m_Follow = playerObj.transform;
            }
        }
        

    }

}
