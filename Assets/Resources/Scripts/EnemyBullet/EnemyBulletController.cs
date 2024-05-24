using Unity.Netcode;
using UnityEngine;

public class EnemyBulletController : NetworkBehaviour {
    private PlayerHealth playerHealth;
    [SerializeField]
    private int damage;
    private GameObject explosion;
    private int iMulti;
    
    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        Destroy(gameObject, 4f);
        playerHealth = FindObjectOfType<PlayerHealth>();
        explosion = Resources.Load<GameObject>("Prefabs/Effects/BulletExplosionEffect1");
        var velocity = GetComponent<Rigidbody2D>().velocity;
        SetVelocity(velocity* Time.deltaTime * 100);
    }

    // Detects if the collision is an object Foreground or player
    void OnTriggerEnter2D(Collider2D other) {
        if(iMulti == 0)
            EnemyInteract(other);
        else
            EnemyInteractNet(other);
        
    }

    void EnemyInteract(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.decreaseHealth(damage);
                DamagePopup.Create(transform.position, damage, false);
            }

        }

        if (!other.CompareTag("Enemy") &&
            !other.CompareTag("Bullet") &&
            !other.CompareTag("Boss") &&
            !other.CompareTag("Ignore") &&
            !other.CompareTag("Hydra") &&
            !other.CompareTag("Hole") &&
            !other.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    void EnemyInteractNet(Collider2D other)
    {

        if (!IsServer)
            return;

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.decreaseHealth(damage);
                DamagePopupServerRpc(damage);
            }
        }

        if (!other.CompareTag("Enemy") &&
            !other.CompareTag("Bullet") &&
            !other.CompareTag("Boss") &&
            !other.CompareTag("Ignore") &&
            !other.CompareTag("Hydra") &&
            !other.CompareTag("Hole") &&
            !other.CompareTag("EnemyBullet"))
        {

        if (IsSpawned)
            BulletDestroyServerRpc();        

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

    public void SetVelocity(Vector2 velocity)
    {
        if (IsServer)
        {
            var bulletRb = GetComponent<Rigidbody2D>();
            bulletRb.velocity = velocity;
            SetVelocityClientRpc(velocity);
        }
    }

    [ClientRpc]
    void SetVelocityClientRpc(Vector2 velocity)
    {
        if (!IsHost)
        {
            var bulletRb = GetComponent<Rigidbody2D>();
            bulletRb.velocity = velocity;
        }
    }

    [ServerRpc]
    void BulletDestroyServerRpc()
    {
        NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        NetworkObjectSpawner.SpawnNewNetworkObject(explosion, transform.position, transform.rotation);
    }
}
