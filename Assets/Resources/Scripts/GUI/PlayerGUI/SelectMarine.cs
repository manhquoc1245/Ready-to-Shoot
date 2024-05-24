using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMarine : MonoBehaviour {

    [SerializeField]
    private CharacterDataSO[] characterDataSO;

    public void SelectPlayer1() {
        characterDataSO[0].isSelected = true;
        characterDataSO[1].isSelected = false;
        characterDataSO[2].isSelected = false;
        characterDataSO[3].isSelected = false;
        SoundManager.PlaySound("MenuSound");
    }

    public void SelectPlayer2() {
        characterDataSO[0].isSelected = false;
        characterDataSO[1].isSelected = true;
        characterDataSO[2].isSelected = false;
        characterDataSO[3].isSelected = false;
        SoundManager.PlaySound("MenuSound");
    }
    
    public void SelectPlayer3() {
        characterDataSO[0].isSelected = false;
        characterDataSO[1].isSelected = false;
        characterDataSO[2].isSelected = true;
        characterDataSO[3].isSelected = false;
        SoundManager.PlaySound("MenuSound");
    }

    public void SelectPlayer4() {
        characterDataSO[0].isSelected = false;
        characterDataSO[1].isSelected = false;
        characterDataSO[2].isSelected = false;
        characterDataSO[3].isSelected = true;
        SoundManager.PlaySound("MenuSound");
    }
}
