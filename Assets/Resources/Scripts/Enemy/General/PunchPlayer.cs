using UnityEngine;
using Unity.Netcode;

public class PunchPlayer : NetworkBehaviour {
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool isAttacking;
    
    public bool hitAnimation;
    public int damage;
    public float distance;
    private int iMulti;
    
    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        animator = GetComponent<Animator>();
    }

    void Update() {
        playerHealth = FindObjectOfType<PlayerHealth>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        if (hitAnimation)
            HitPlayerWithAnimation();
    }

    void HitPlayerWithAnimation() {
        if (Vector2.Distance(transform.position, playerTransform.position) < distance && !isAttacking) {
            isAttacking = true;
            animator.SetBool("Attacking", true);
        } else if (Vector2.Distance(transform.position, playerTransform.position) > distance && isAttacking) {
            isAttacking = false;
            animator.SetBool("Attacking", false);
        }
    }

    void HitPlayerWithOutAnimation() {
        if (Vector2.Distance(transform.position, playerTransform.position) < distance)
            HitPlayer();
    }

    void HitPlayer()
    {
        if (iMulti == 0)
            playerHealth.decreaseHealth(damage);
        else
        {
            if (!IsServer) 
                return;
            playerHealth.decreaseHealth(damage);
            DamagePopupServerRpc(damage);
        }
    }
    void NewDamagePopup(int damage)
    {
        DamagePopup.Create(transform.position, damage, false);
    }

    [ServerRpc]
    void DamagePopupServerRpc(int damage)
    {
        DamagePopupClientRpc(damage);

    }

    [ClientRpc]
    void DamagePopupClientRpc(int damage)
    {
        NewDamagePopup(damage);
    }
}
