﻿using UnityEngine;

public class FlipToPlayer : MonoBehaviour {
    private Transform target;
    private bool facingRight;
    
    void Start() {
        
        facingRight = true;
    }

    void Update() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            target = player.GetComponent<Transform>();
        }
        flipEnemySprite();
    }

    void flipEnemySprite() {
        if (Mathf.Abs(getAngleRelToPlayer()) < 90 && !facingRight) {
            Flip();
        } else if (Mathf.Abs(getAngleRelToPlayer()) > 90 && facingRight) {
            Flip();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);
    }

    float getAngleRelToPlayer() {
        // Get vector relative to enemy and player
        Vector2 aim = gameObject.transform.position - target.transform.position;
        aim *= -1f;

        // Get angle of the aim vector
        return Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
    }
}
