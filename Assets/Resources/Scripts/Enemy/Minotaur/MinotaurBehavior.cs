using UnityEngine;

public class MinotaurBehavior : MonoBehaviour {
    private Animator animator;
    private EnemyHealth enemyHealth;
    
    void Start() {
        animator = transform.Find("EnemySprite").GetComponent<Animator>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
    }

    void Update() {
        if (enemyHealth.GetHealth() <= 0) {
            animator.SetBool("IsDying", true);
        }
    }
}
