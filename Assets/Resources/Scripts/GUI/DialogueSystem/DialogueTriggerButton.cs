using UnityEngine;

public class DialogueTriggerButton : MonoBehaviour {
    public Dialogue dialogue;
    public float distance;
    private bool isActive;

    private Transform playerTransform;
    
    void Start() {
        
    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void Update() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        

        if (Input.GetKeyDown(KeyCode.Q) && Vector2.Distance(transform.position, playerTransform.position) < distance && !isActive) {
            isActive = true;
            SoundManager.PlaySound("Typing");
            TriggerDialogue();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isActive) {
            isActive = !isActive;
        }
    }
}
