using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayTutorial() {
        PlayMenuSound();
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayLevel1() {
        PlayMenuSound();
        SceneManager.LoadScene("Level1");
    }
    
    public void PlayLevel2() {
        PlayMenuSound();
        Debug.LogWarning("Scene for Level 2 does not exist");
    }

    public void PlayLevel3() {
        PlayMenuSound();
        Debug.LogWarning("Scene for Level 3 does not exist");
    }

    public void QuitGame() {
        PlayMenuSound();
        Application.Quit();
    }

    public void MultiScene()
    {
        PlayerPrefs.SetInt("Multiplayer", 1);
        SceneManager.LoadScene("Menu");
    }

    public void PlayScene()
    {
        PlayerPrefs.SetInt("Multiplayer", 0);
    }

    public void PlayMenuSound() {
        SoundManager.PlaySound("MenuSound");
    }
}
