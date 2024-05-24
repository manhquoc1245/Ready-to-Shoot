using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "StructData/CharacterData")]
public class PlayerData_SO : ScriptableObject
{
    public int id;
    public string playerName;
    public int maxHealth;
    public GameObject player;

    public PlayerData GetDataInstance()
    {
        return new PlayerData()
        {
            id = this.id,
            playerName = this.playerName,
            maxHealth = this.maxHealth,
            player = this.player,
        };
    }
}
