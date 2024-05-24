using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.Netcode;
using System;
using System.Xml;
using System.Runtime.InteropServices.WindowsRuntime;

public class GunsInventory : NetworkBehaviour {
    List<GameObject> gunsInInventory = new List<GameObject>();
    private GameObject pickedGun;
    private int activeWeapon = 0;
    private bool wait = true;
    Rotate ro;
    private float aim;
    private Vector3 shootPosition;
    private Quaternion shootRotation;
    private GameObject bullet;
    private GameObject muzzle;
    private bool heldon;
    [SerializeField]
    private GameObject hand;
    Vector3 handPosition;
    // For the interface
    private Image gunImage;
    private GameObject cartridgePanel;
    private GameObject ammoBagPanel;
    private PlayerMovement pm;
    private float bulletForce = 10f;
    private int iMulti;

    Rotate m_Rotate;

    void Awake()
    {
    }

    void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        muzzle = Resources.Load<GameObject>("Prefabs/Effects/ShotEffect1");
    }

    void Update() {     
        

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


    }

    public void GuninventoryMain()
    {
        gunImage = GameObject.Find("GunImage").GetComponent<Image>();
        cartridgePanel = GameObject.Find("CartridgePanel").transform.Find("Image").gameObject;
        ammoBagPanel = GameObject.Find("AmmoBagPanel").transform.Find("Image").gameObject;

        float deltaScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (deltaScroll != 0 && wait && gunsInInventory.Count > 1)
        {
            StartCoroutine(waitToChange());
            if (deltaScroll < 0)
                ChangeWeapon(1);
            if (deltaScroll > 0)
                ChangeWeapon(-1);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon(1);
        }
        if (gunsInInventory.Count > 0)
        {
            Rotate ro = gunsInInventory[activeWeapon].GetComponent<Rotate>();
            if (ro != null && hand != null)
            {
                ro.SetOwnerHand(hand);
            }
        }
    }

    IEnumerator waitToChange() {
        wait = false;
        yield return new WaitForSeconds(0.25f);
        wait = true;
    }
    void ChangeWeapon(int delta) {

        if (gunsInInventory.Count == 0)
            return;

        gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
        gunsInInventory[activeWeapon].SetActive(false);

        if (activeWeapon == 0 && delta < 0)
            activeWeapon = gunsInInventory.Count - 1;
        else if (activeWeapon == gunsInInventory.Count - 1 && delta > 0)
            activeWeapon = 0;
        else
            activeWeapon += delta;

        gunsInInventory[activeWeapon].transform.position = hand.transform.position;
        gunsInInventory[activeWeapon].GetComponent<Shoot>().RestartValues();
        gunsInInventory[activeWeapon].SetActive(true);
        UpdateGUI();
        gunsInInventory[activeWeapon].GetComponent<Shoot>().UpdateGUI();

        SoundManager.PlaySound("ShotgunLoad");
    }

    
    void OnTriggerEnter2D(Collider2D other) {
        if(iMulti == 0)
            GuninventoryInteract(other);
        else
            GuninventoryInteractNet(other);

    }

    public void GuninventoryInteract(Collider2D other)
    {
        if (other.gameObject.CompareTag("ItemGun"))
        {
            string path = "Prefabs/Guns/Gun" + Regex.Replace(other.gameObject.name, "[^.0-9]", "");
            pickedGun = (GameObject)Resources.Load(path, typeof(GameObject));

            if (gunsInInventory.Count > 0)
            {
                gunsInInventory[activeWeapon].SetActive(false);
                gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
            }

            GameObject gunO = Instantiate(pickedGun, hand.transform.position, Quaternion.identity);

            gunsInInventory.Add(gunO);

            activeWeapon = gunsInInventory.Count - 1;

            Destroy(other.gameObject);

            UpdateGUI();
            ammoBagPanel.SetActive(true);
            cartridgePanel.SetActive(true);

            SoundManager.PlaySound("LeatherInventory");

            //Debug.Log(gunsInInventory.Count);
        }
    }

    public void GuninventoryInteractNet(Collider2D other)
    {
        if (other.gameObject.CompareTag("ItemGun"))
        {
            string path = "Prefabs/Guns/Gun" + Regex.Replace(other.gameObject.name, "[^.0-9]", "");
            pickedGun = (GameObject)Resources.Load(path, typeof(GameObject));

            if (gunsInInventory.Count > 0)
            {
                gunsInInventory[activeWeapon].SetActive(false);
                gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
            }

            GameObject gunO = Instantiate(pickedGun, hand.transform.position, Quaternion.identity);
            //GameObject gunO = NetworkObjectSpawner.SpawnNewNetworkObject(pickedGun, hand.transform.position, Quaternion.identity); ;
            gunO.transform.SetParent(transform);

            gunsInInventory.Add(gunO);
            activeWeapon = gunsInInventory.Count - 1;

            if(IsOwner)
                Destroy(other.gameObject);

            UpdateGUI();
            ammoBagPanel.SetActive(true);
            cartridgePanel.SetActive(true);

            SoundManager.PlaySound("LeatherInventory");

            //Debug.Log(gunsInInventory.Count);
        }
    }

    void UpdateGUI() {
        gunImage.sprite = gunsInInventory[activeWeapon].transform.Find("Gun").GetComponent<SpriteRenderer>().sprite;
        gunImage.color = new Color(gunImage.color.r, gunImage.color.g, gunImage.color.b, 1f);
    }

    public GameObject GetActiveWeapon() {
        if (gunsInInventory.Count > 0)
            return gunsInInventory[activeWeapon];
        return null;
    }

    public void GuninventoryMainNet()
    {
        if (!IsLocalPlayer) return;

        gunImage = GameObject.Find("GunImage").GetComponent<Image>();
        cartridgePanel = GameObject.Find("CartridgePanel").transform.Find("Image").gameObject;
        ammoBagPanel = GameObject.Find("AmmoBagPanel").transform.Find("Image").gameObject;
        handPosition = hand.transform.position;
        float deltaScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (deltaScroll != 0 && wait && gunsInInventory.Count > 1)
        {
            StartCoroutine(waitToChange());
            if (deltaScroll < 0)
                ChangeWeaponNet(1);
            if (deltaScroll > 0)
                ChangeWeaponNet(-1);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeaponNet(1);
        }
        handPosition = hand.transform.position;
        
        if(gunsInInventory.Count > 0)
        {
            aim = gunsInInventory[activeWeapon].GetComponent<Rotate>().followAimPoint();
            shootPosition = gunsInInventory[activeWeapon].GetComponent<Shoot>().shootPosition;
            shootRotation = gunsInInventory[activeWeapon].GetComponent<Shoot>().shootRotation;
            string path = gunsInInventory[activeWeapon].GetComponent<Shoot>().bulletPath;
            bullet = Resources.Load<GameObject>(path);
            GunSyncServerRpc(activeWeapon, aim);
            //GunMove();
            gunsInInventory[activeWeapon].GetComponent<Shoot>().ActiveShoot();
            //if (gunsInInventory[activeWeapon].GetComponent<Shoot>().isShoot)
                //PlayerOwnerShootGunServerRpc();
            //heldon = gunsInInventory[activeWeapon].GetComponent<Shoot>().HeldOn;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            //PlayerOwnerShootGunServerRpc(activeWeapon);
        }
    }

    public void AcS()
    {
        gunsInInventory[activeWeapon].GetComponent<Shoot>().ActiveShoot();
    }

    [ServerRpc]
    public void PlayerOwnerShootGunServerRpc()
    {
        gunsInInventory[activeWeapon].GetComponent<Shoot>().ShootNewBulletNet();
        gunsInInventory[activeWeapon].GetComponent<Shoot>().NewBulletVfxNet();
    }

    [ServerRpc]
    void GunSyncServerRpc(int aw, float aim)
    {
        gunsInInventory[aw].GetComponent<Rotate>().MoveWithHandNet(hand.transform);
        gunsInInventory[aw].GetComponent<Rotate>().rotateGun(aim);
        GunSyncClientRpc(aw, aim);
    }
    [ClientRpc]
    void GunSyncClientRpc(int aw, float aim)
    {
        gunsInInventory[aw].GetComponent<Rotate>().MoveWithHandNet(hand.transform);
        gunsInInventory[aw].GetComponent<Rotate>().rotateGun(aim);
    }

    public void ChangeWeaponNet(int delta)
    {
        
        if (gunsInInventory.Count == 0)
            return;

        gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
        ChangeWeaponNetServerRpc(activeWeapon, false, handPosition);

        if (activeWeapon == 0 && delta < 0)
            activeWeapon = gunsInInventory.Count - 1;
        else if (activeWeapon == gunsInInventory.Count - 1 && delta > 0)
            activeWeapon = 0;
        else
            activeWeapon += delta;

 
        gunsInInventory[activeWeapon].GetComponent<Shoot>().RestartValues();
        ChangeWeaponNetServerRpc(activeWeapon, true, handPosition);
        UpdateGUI();
        gunsInInventory[activeWeapon].GetComponent<Shoot>().UpdateGUI();

    }

    [ServerRpc]
    public void ChangeWeaponNetServerRpc(int activeWeapon, bool hasGun, Vector3 position)
    {

        gunsInInventory[activeWeapon].SetActive(hasGun);
        gunsInInventory[activeWeapon].transform.position = position;
        SoundManager.PlaySound("ShotgunLoad");
        ChangeWeaponNetClientRpc(activeWeapon,hasGun,position);

    }

    [ClientRpc]
    public void ChangeWeaponNetClientRpc(int activeWeapon, bool hasGun, Vector3 position)
    {
        gunsInInventory[activeWeapon].transform.position = position;
        gunsInInventory[activeWeapon].SetActive(hasGun);
        SoundManager.PlaySound("ShotgunLoad");

    }

}
