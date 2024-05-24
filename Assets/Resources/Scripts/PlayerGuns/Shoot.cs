using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Unity.Netcode;
using System.Xml;

public class Shoot : NetworkBehaviour {
    // Bullet
    [SerializeField]
    private Transform shootPoint;
    private GameObject bullet;
    private float bulletForce = 25f;
    private string path;

    public float shootWaitTime;
    private bool wait;
    private float startTime;
    private float endTime;

    private bool heldOn;

    private GameObject muzzle1;

    // For Ammunition
    private int remainingBulletsInCartridge;
    public int cartridgeSize;
    private int remainingBulletsTotal;
    public int ammobagSize;
    public float reloadTime;
    private bool reloading;

    private Text cartridgeTxt;
    private Text ammoBagTxt;

    private GameObject reloadIndicator;
    //private GameObject bulletInstantiated;
    NetworkObjectPool m_ObjectPool;

    private int iMulti;
    
    void Start() {
        iMulti = PlayerPrefs.GetInt("Multiplayer");
        cartridgeTxt = GameObject.Find("CartridgeTxt").GetComponent<Text>();
        ammoBagTxt = GameObject.Find("AmmoBagTxt").GetComponent<Text>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 )
        {
            foreach (GameObject player in players)
            {
                reloadIndicator = player.transform.Find("ReloadIndicator").gameObject;
            }

        }
        SelectBullet();

        // Get shootpoint transform and load bullet prefab
        bullet = Resources.Load<GameObject>(path);
        muzzle1 = Resources.Load<GameObject>("Prefabs/Effects/ShotEffect1");

        wait = true;

        remainingBulletsInCartridge = cartridgeSize;
        remainingBulletsTotal = ammobagSize;
        UpdateGUI();
    }

    public void RestartValues() {
        heldOn = false;
        wait = true;
    }

    void Update() {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void ActiveShoot()
    {

        if (Input.GetButtonDown("Fire1"))
            heldOn = true;

        if (Input.GetButtonUp("Fire1"))
            heldOn = false;

        if (Time.time > endTime && wait == false)
            wait = true;

        if ((Input.GetButtonDown("Fire1") || heldOn) && wait && Time.timeScale != 0 && remainingBulletsInCartridge > 0 && !reloading)
        {
            if(iMulti == 1)
            {
                gameObject.GetComponentInParent<GunsInventory>().PlayerOwnerShootGunServerRpc();
                remainingBulletsInCartridge--;
                UpdateGUI();
                startTime = Time.time;
                endTime = startTime + shootWaitTime;
                wait = false;
            }
            else
            {
                ShootBullet();
                startTime = Time.time;
                endTime = startTime + shootWaitTime;
                wait = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.R) || (remainingBulletsInCartridge == 0 && Input.GetButtonDown("Fire1"))) &&
            !reloading &&
            remainingBulletsInCartridge < cartridgeSize &&
            remainingBulletsTotal > 0)
        {
            StartCoroutine(ReloadTime());
        }
    }

    public void UpdateGUI() {
        cartridgeTxt.text = $"{remainingBulletsInCartridge}/{cartridgeSize}";
        ammoBagTxt.text = $"{remainingBulletsTotal}/{ammobagSize}";
    }

    private IEnumerator ReloadTime() {
        reloading = true;
        reloadIndicator.SetActive(true);
        SoundManager.PlaySound("Reload1");
        yield return new WaitForSeconds(reloadTime);
        ReloadCartridge();
        UpdateGUI();
        reloading = false;
        reloadIndicator.SetActive(false);
    }

    private void ReloadCartridge() {
        if (remainingBulletsTotal > 0 && remainingBulletsInCartridge < cartridgeSize)
            if (cartridgeSize - remainingBulletsInCartridge > remainingBulletsTotal) {
                remainingBulletsInCartridge += remainingBulletsTotal;
                remainingBulletsTotal = 0;
            } else if (remainingBulletsInCartridge > 0) {
                remainingBulletsTotal -= cartridgeSize - remainingBulletsInCartridge;
                remainingBulletsInCartridge = cartridgeSize;
            } else {
                remainingBulletsInCartridge = cartridgeSize;
                remainingBulletsTotal -= cartridgeSize;
            }
    }

    public void FillAmmo() {
        SoundManager.PlaySound("Reload1");
        remainingBulletsInCartridge = cartridgeSize;
        remainingBulletsTotal = ammobagSize;
        UpdateGUI();
    }

    // Choose bullet depending on gun
    public string SelectBullet() {
        path = "Prefabs/Bullets/Bullet" + Regex.Replace(gameObject.name, "[^.0-9]", "");
        return path;
    }

    // Instantiate bullet
    void ShootBullet() {
        GameObject bulletInstantiated = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        GameObject muzzle = Instantiate(muzzle1, shootPoint.position, shootPoint.rotation);
        muzzle.transform.SetParent(shootPoint);

        Rigidbody2D bulletRB = bulletInstantiated.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);

        remainingBulletsInCartridge--;
        UpdateGUI();

        SoundManager.PlaySound("Shoot2");
    }

    public void ShootNewBulletNet()
    {
        GameObject bulletInstantiated = NetworkObjectSpawner.SpawnNewNetworkObject(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletRB = bulletInstantiated.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootPoint.right * bulletForce, ForceMode2D.Impulse);
        var velocity = bulletRB.velocity;
        var bulletIns = bulletInstantiated.GetComponent<BulletController>();
        bulletIns.SetVelocity(velocity);
        

        SoundManager.PlaySound("Shoot2");
    }
    [ServerRpc]
    public void ShootNewBulletNetServerRpc()
    {
        GameObject bulletInstantiated = NetworkObjectSpawner.SpawnNewNetworkObject(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletRB = bulletInstantiated.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(shootPoint.right * bulletForce * Time.deltaTime, ForceMode2D.Impulse);
        var velocity = bulletRB.velocity;
        var bulletIns = bulletInstantiated.GetComponent<BulletController>();
        bulletIns.SetVelocity(velocity);
        remainingBulletsInCartridge--;
        UpdateGUI();

        SoundManager.PlaySound("Shoot2");
    }

    public void DestroyBullet()
    {
        //Destroy(bulletInstantiated);
    }
    public void NewBulletVfxNet()
    {
        NetworkObjectSpawner.SpawnNewNetworkObject(muzzle1, shootPoint.position, shootPoint.rotation);
    }

    public string bulletPath {
        get { return SelectBullet(); }
    }

    public bool Reloading {
        set { 
            reloading = value;
            reloadIndicator.SetActive(value);
        }
    }

    public bool AbleToRefill {
        get {
            if (remainingBulletsTotal < ammobagSize || remainingBulletsInCartridge < cartridgeSize)
                return true;
            else
                return false;
        }
    }

    public bool HeldOn {  get { return heldOn; } }

    public Vector3 shootPosition { get { return shootPoint.position; } }

    public Quaternion shootRotation { get { return shootPoint.rotation; } }
}
