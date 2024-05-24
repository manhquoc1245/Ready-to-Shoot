using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllCharacter", menuName = "StructData/CharactersData")]
public class AllPlayerData_SO : ScriptableObject
{
    public List<PlayerData_SO> characters;
}
