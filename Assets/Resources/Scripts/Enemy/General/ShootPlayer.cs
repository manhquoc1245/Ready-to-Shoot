using System.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class ShootPlayer : NetworkBehaviour {
    // Bullet
    private Transform shootPoint;
    public GameObject bullet;
    private float bulletForce = 10f;
    private string path;
    public string TypeOfBullet;

    private bool ableToShoot;

    public bool shootFromAnimator;
    
    public float waitToShoot;

    private float aimAngle;

    private Transform target;

    private int iMulti;
    
    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        ableToShoot = true;

        // Get shootpoint transform and load bullet prefab
        shootPoint = transform.Find("Cannon");

        path = "Prefabs/EnemyBullets/" + TypeOfBullet;

        bullet = (GameObject) Resources.Load(path, typeof(GameObject));

    }

    void Update() {
        if (iMulti == 1)
        {
            if (!IsServer) return;
        }
        target = FindAnyObjectByType<PlayerMovement>().transform;
        if (ableToShoot && !shootFromAnimator) { 
            StartCoroutine(ShootBulletCoroutine());
            ableToShoot = false;
        }
        
        rotateCannon();
    }

    IEnumerator ShootBulletCoroutine() {
        yield return new WaitForSeconds(waitToShoot);
        if (iMulti == 1)
            ShootBulletNetServerRpc();
        else
            ShootBullet();
        ableToShoot = true;
    }

    void ShootBullet() {
        GameObject bulletInstantiated = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletRB = bulletInstantiated.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);
    }

    // Move Aim Point
    void rotateCannon() {
        // Get vector relative to enemy and player
        Vector2 aim = gameObject.transform.position - target.position;
        aim *= -1f;

        // Get angle of the aim vector
        aimAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        shootPoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    public string bulletPath {
        get { return path; }
    }

    [ServerRpc]
    void ShootBulletNetServerRpc()
    {
        GameObject bulletInstantiated = NetworkObjectSpawner.SpawnNewNetworkObject(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletRB = bulletInstantiated.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
