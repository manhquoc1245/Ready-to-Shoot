using Unity.Netcode;
using UnityEngine;

public class BulletController : NetworkBehaviour {
    private EnemyHealth enemyHealth;
    [SerializeField]
    private int minDamage, maxDamage;
    private int damage;
    private bool isCritical;
    private GameObject explosion;
    private int iMulti;

    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        Destroy(gameObject, 4f);
        explosion = Resources.Load<GameObject>("Prefabs/Effects/BulletExplosionEffect1");
        damage = Random.Range(minDamage, maxDamage);
        isCritical = Random.Range(0, 100) < 30;
        if (isCritical) damage *= 2;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (iMulti == 1)
        {
            if (!IsServer || !IsSpawned) 
                return;
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out IDamagable damagable))
                {
                    damagable.decreaseHealth(damage);
                    DamagePopupServerRpc(damage);
                }
            }

            if (other.CompareTag("Boss"))
            {
                BossHealth bossHealth = other.GetComponent<BossHealth>();
                bossHealth.TakeDamage(damage);
            }

            if (other.CompareTag("Hydra"))
            {
                HydraHealth bossHealth = other.GetComponent<HydraHealth>();
                bossHealth.TakeDamage(damage);
            }

            if (!other.CompareTag("Player") &&
                !other.CompareTag("PlayerChild") &&
                !other.CompareTag("Bullet") &&
                !other.CompareTag("Hole") &&
                !other.CompareTag("EnemyBullet") &&
                !other.CompareTag("Ignore"))
            {
                BulletDestroyServerRpc();
                SoundManager.PlaySound("Explosion");
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.decreaseHealth(damage);
                DamagePopup.Create(enemyHealth.GetPosition(), damage, isCritical);
            }

            if (other.CompareTag("Boss"))
            {
                BossHealth bossHealth = other.GetComponent<BossHealth>();
                bossHealth.TakeDamage(damage);
            }

            if (other.CompareTag("Hydra"))
            {
                HydraHealth bossHealth = other.GetComponent<HydraHealth>();
                bossHealth.TakeDamage(damage);
            }

            if (!other.CompareTag("Player") &&
                !other.CompareTag("PlayerChild") &&
                !other.CompareTag("Bullet") &&
                !other.CompareTag("Hole") &&
                !other.CompareTag("EnemyBullet") &&
                !other.CompareTag("Ignore"))
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);
                SoundManager.PlaySound("Explosion");
            }
        }
    }

    void NewDamagePopup(int damagev)
    {
        DamagePopup.Create(transform.position, damagev, false);
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

    [ServerRpc]
    void BulletDestroyServerRpc()
    {
        NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        NetworkObjectSpawner.SpawnNewNetworkObject(explosion, transform.position, transform.rotation);
    }

    public void BulletDestroy()
    {
        if (!NetworkObject.IsSpawned) return;
        NetworkObject.Despawn(true);
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

    public int Damage {
        get { return damage; }
    }
}
