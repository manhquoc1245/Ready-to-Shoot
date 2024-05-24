using UnityEngine;

public class OutlineChanger : MonoBehaviour {
    private SpriteRenderer spriteR;
    public float distance;
    private bool isActive;

    public Material m1;
    public Material m2;

    private Transform playerTransform;
    
    void Start() {
        spriteR = GetComponent<SpriteRenderer>();
    }

    void Update() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        if (Vector2.Distance(transform.position, playerTransform.position) < distance && !isActive) {
            spriteR.material = m2;
            isActive = !isActive;
        } else if (Vector2.Distance(transform.position, playerTransform.position) > distance && isActive) {
            isActive = !isActive;
            spriteR.material = m1;
        }
    }
}
