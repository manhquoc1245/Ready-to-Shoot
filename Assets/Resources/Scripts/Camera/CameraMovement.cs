using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private Transform playerTransform;
    private Vector3 playerPosition;

    void Start() {

    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            playerTransform = player.GetComponent<Transform>();
        }

        playerPosition = playerTransform.position;
        playerPosition.z = -10;
        transform.position = playerPosition;
    }
}
