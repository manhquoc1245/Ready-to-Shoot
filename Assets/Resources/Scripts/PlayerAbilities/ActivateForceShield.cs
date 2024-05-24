using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ActivateForceShield : MonoBehaviour {
    private GameObject forceShield;
    private CapsuleCollider2D bc2d;
    private PlayerHealth playerHealth;
    private bool wait;

    [Header("Force shield config")]
    [Range(0f, 100f)]
    [SerializeField]
    private float shieldExpireTime;

    [Range(0f, 100f)]
    [SerializeField]
    private float shieldNextActiveTime;


    void Start() {
        forceShield = Resources.Load<GameObject>("Prefabs/PlayerAbilities/ForceShield");
        bc2d = GetComponent<CapsuleCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        wait = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ActiveShield()
    {
        if (Input.GetButtonDown("Fire2") && wait && Time.timeScale != 0)
        {
            StartCoroutine(playerInvinsible());
            GameObject shieldInstance = Instantiate(forceShield, transform.position, transform.rotation);
            shieldInstance.transform.parent = gameObject.transform;
            Destroy(shieldInstance, shieldExpireTime);
        }
    }

    IEnumerator playerInvinsible() {
        playerHealth.invinsible = true;
        wait = false;
        yield return new WaitForSeconds(shieldExpireTime);
        playerHealth.invinsible = false;
        yield return new WaitForSeconds(shieldNextActiveTime);
        wait = true;
    }
}
