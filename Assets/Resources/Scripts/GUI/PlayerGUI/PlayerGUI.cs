using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;

public class PlayerGUI : NetworkBehaviour {

    [SerializeField]
    private CharacterDataSO[] m_charactersData;

    [SerializeField]
    private Image playerAvatar;
    private int iMulti;
    private bool pauseMenuActive;
    private bool inventoryMenuActive;
    public GameObject pauseMenuBackground;
    public GameObject inventoryMenu;
    public GameObject pauseMenu;
    public GameObject dialogueMenu;
    public GameObject gameOverMenu;

    private Scene activeScene;
    private DialogueManager dialogueManager;
    private PlayerHealth playerHealth;
    
    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        pauseMenuActive = false;
        inventoryMenuActive = false;
        activeScene = SceneManager.GetActiveScene();

        dialogueManager = FindObjectOfType<DialogueManager>(); 

        pauseMenuBackground.SetActive(false);
        if(iMulti == 1)
        {
            if (NetworkManager.ConnectedClients.ContainsKey(OwnerClientId))
            {
                var client = NetworkManager.ConnectedClients[OwnerClientId];
                foreach (CharacterDataSO data in m_charactersData)
                {
                    if (data.isSelected && data.clientId == client.ClientId)
                    {
                        playerAvatar.sprite = data.iconAvatar;
                    }

                }
            }
        }
        else
        {
            foreach (CharacterDataSO data in m_charactersData)
            {
                if (data.isSelected)
                {
                    playerAvatar.sprite = data.iconAvatar;
                }

            }
        }
        
    }
    
    void Update() {

        playerHealth = FindObjectOfType<PlayerHealth>();

        bool dialogueCheck = dialogueManager.DialoguePanelIsActive;

        if (Input.GetKeyDown(KeyCode.Escape) && !inventoryMenuActive && !dialogueCheck && !playerHealth.IsKill)
            ShowPauseMenu();

        if (Input.GetKeyDown(KeyCode.E) && !pauseMenuActive && !dialogueCheck && !playerHealth.IsKill)
            ShowInventory();

        if (playerHealth.IsKill && Time.timeScale != 0) {
            Time.timeScale = 0;
            pauseMenuBackground.SetActive(true);
            Cursor.visible = true;
            gameOverMenu.SetActive(true);
            FindObjectOfType<SoundManager>().StopAllSongs();
            SoundManager.PlaySound("GameOver");
        }
    }

    public void ShowPauseMenu() {
        PlayMenuSound();
        pauseMenuActive = !pauseMenuActive;
        pauseMenuBackground.SetActive(pauseMenuActive);
        pauseMenu.SetActive(pauseMenuActive);
        Cursor.visible = pauseMenuActive;
        Time.timeScale = pauseMenuActive ? 0 : 1;
    }

    public void ShowInventory() {
        PlayMenuSound();
        
        if (!inventoryMenuActive) {
            inventoryMenuActive = true;
            pauseMenuBackground.SetActive(inventoryMenuActive);
            inventoryMenu.SetActive(inventoryMenuActive);
            Cursor.visible = true;
            Time.timeScale = 0;
        } else {
            inventoryMenuActive = false;
            pauseMenuBackground.SetActive(inventoryMenuActive);
            inventoryMenu.SetActive(inventoryMenuActive);
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void Restart() {
        PlayMenuSound();
        FindObjectOfType<SoundManager>().StopAllSongs();
        ItemsInventory.itemsInInventory.Clear();
        SceneManager.LoadScene(activeScene.name);
    }

    public void Continue() {
        ShowPauseMenu();
    }

    public void ReturnToMenu() {
        PlayMenuSound();
        FindObjectOfType<SoundManager>().StopAllSongs();
        ItemsInventory.itemsInInventory.Clear();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayMenuSound() {
        SoundManager.PlaySound("MenuSound");
    }

    public GameObject DialoguePanel {
        get { return dialogueMenu; }
    }
}
