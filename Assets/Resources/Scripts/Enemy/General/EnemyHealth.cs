using System;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : NetworkBehaviour, IDamagable {
    [SerializeField]
    private int health;
    [SerializeField]
    protected NetworkVariable<int> m_EnemyHealthPoints =
        new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone);
    int iMulti;

    private void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
    }

    public override void OnNetworkSpawn()
    {
        
        if (IsServer)
            m_EnemyHealthPoints.OnValueChanged += OnEnemyHealthPointsChange;
    }

    private void OnDisable()
    {
        if (IsServer)
            m_EnemyHealthPoints.OnValueChanged -= OnEnemyHealthPointsChange;
    }

    public void decreaseHealth(int damage) {

        if (iMulti == 0)
        {
            health -= damage;
        }
        else
        {
            if (!IsServer) 
                return;
            m_EnemyHealthPoints.Value -= damage;
        }
        
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public int GetHealth()
    {
        if (iMulti == 0)
        {
            return health;
        }
        else
        {
            return m_EnemyHealthPoints.Value;
        }
    }
    private void OnEnemyHealthPointsChange(int oldHP, int newHP)
    {
        // if enemy's health is 0, then time to start enemy dead animation
        if (newHP <= 0)
        {
            gameObject.GetComponent<EnemyDeath>().EnemyDieNet();
        }
    }
}
