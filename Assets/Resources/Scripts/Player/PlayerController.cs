using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : NetworkBehaviour
{
    [HideInInspector]
    public CharacterDataSO characterData;

    [HideInInspector]
    public GameplayManager gameplayManager;

    int iMulti;
    private ActivateForceShield shield;
    private ActivateBomb bomb;
    private ActivateTurret turret;
    private ActivateRadialAttack radialAttack;
    private PlayerSprite sprite;
    private PlayerAim aim;
    private GunsInventory inventory;
    private Rotate ro;
    void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");

        shield = gameObject.GetComponent<ActivateForceShield>();

        bomb = gameObject.GetComponent<ActivateBomb>();

        turret = gameObject.GetComponent<ActivateTurret>();

        radialAttack = gameObject.GetComponent<ActivateRadialAttack>();

        sprite = gameObject.GetComponent<PlayerSprite>();

        aim = gameObject.GetComponent<PlayerAim>();

        inventory = gameObject.GetComponent<GunsInventory>();


    }

    void Update()
    {
        if (iMulti == 1)
        {
            if (!IsOwner || !Application.isFocused)
                return;
            sprite.updateSpriteInRotation();

            aim.MoveAimPoint();

            inventory.GuninventoryMainNet();

            //inventory.AcS();

        }
        else
        {
            sprite.updateSpriteInRotation();

            aim.MoveAimPoint();

            inventory.GuninventoryMain();
        }

        if (shield != null)
            shield.ActiveShield();

        if(bomb != null)
            bomb.ActiveBomb();

        if (turret != null)
            turret.ActiveTurret();

        if (radialAttack != null)
            radialAttack.ActiveRadialAttack();     

    }
}


