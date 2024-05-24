using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour, IDamagable{
    public NetworkVariable<int> healthNet = new NetworkVariable<int>();
    private int health;
    private bool isKill;

    [HideInInspector]
    public bool invinsible;

    private HealthBar healthBar;

    private GameObject blood1;

    private GameObject bloodHeal1;

    private int iMulti;

    [Header("Player health config")]
    [SerializeField] private int baseHealth;

    [Header("Runtime set")]
    [HideInInspector]
    public PlayerUI playerUI;


    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        if(iMulti == 0)
        {
            health = baseHealth;
            isKill = false;
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            healthBar.SetMaxHealth(health);
        }
        
        blood1 = Resources.Load<GameObject>("Prefabs/Effects/BloodEffect1");
        bloodHeal1 = Resources.Load<GameObject>("Prefabs/Effects/HealEffect1");
    }

    void Update() {
        if (iMulti == 1)
        {
            if (!IsServer)
            {
                return;
            }

            if (healthNet.Value <= 0 && !isKill)
            {
                isKill = true;
            }
        }
        else
        {
            if (health == 0 && !isKill)
            {
                isKill = true;
            }
        }
    }

    public void decreaseHealth(int damage) {

        if (iMulti == 1)
        {
            if (!IsServer)
            {
                return;
            }

            healthNet.Value -= damage;
            if (!invinsible)
            {
                playerUI.UpdateHealth(healthNet.Value);
                GameObject blood = Instantiate(blood1, transform.position, transform.rotation);
                blood.transform.SetParent(transform);
                SoundManager.PlaySound("Hurt");

            }
        }
        else
        {
            health = Math.Max(0, health - damage);
            if (!invinsible)
            {
                healthBar.SetHealth(health);
                GameObject blood = Instantiate(blood1, transform.position, transform.rotation);
                blood.transform.SetParent(transform);
                SoundManager.PlaySound("Hurt");

            }
        }
    }

    public void IncreaseHealth(int heal) {

        if (iMulti == 1)
        {
            if (!IsServer)
            {
                return;
            }
            healthNet.Value += heal;
            playerUI.UpdateHealth(healthNet.Value);
            GameObject blood = Instantiate(bloodHeal1, transform.position, transform.rotation);
            blood.transform.SetParent(transform);
        }
        else
        {
            health = Math.Min(baseHealth, health + heal);
            healthBar.SetHealth(health);
            GameObject blood = Instantiate(bloodHeal1, transform.position, transform.rotation);
            blood.transform.SetParent(transform);
        }
        
    }

    public bool IsKill {
        get { return isKill; }
    }
}
