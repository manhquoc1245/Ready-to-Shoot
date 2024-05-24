using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour {
    public Slider slider;

    private int iMulti;

    private void Awake()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        if(iMulti == 1)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetMaxHealth(int health) {

        if(iMulti == 1)
        {
            if (!IsOwner) return;
        }

        slider.maxValue = health;
        slider.value = health;
        Debug.Log(health.ToString() + " d");
    }

    public void SetHealth(int health) {

        if (iMulti == 1)
        {
            if (!IsOwner) return;
        }

        slider.value = health;
        Debug.Log(health.ToString() + " e");
    }
}
