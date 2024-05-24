using Unity.Netcode;
using UnityEngine;

public class Rotate : NetworkBehaviour {
    // Gun
    private GameObject gun;
    private Rigidbody2D rb;

    // Bullet
    private GameObject bullet;
    private Shoot shoot;
    
    // Player hand
    private GameObject hand;
    private Vector3 handPosition;
    
    // Gun Rotation
    private bool isFlipped = false;

    private GameObject muzzle1;

    void Start() {
        // Get RigidBody of the gun
        rb = gameObject.GetComponent<Rigidbody2D>();
        // Get hand of player
        // Get gun
        gun = GameObject.Find("Gun");

        // TODO: If statement for that changes bullet depending on gun
        // Load bullet prefab
        shoot = gameObject.GetComponent<Shoot>();
        bullet = Resources.Load<GameObject>(shoot.bulletPath);
        muzzle1 = Resources.Load<GameObject>("Prefabs/Effects/ShotEffect1");


    }

    public void SetOwnerHand(GameObject setHand)
    {
        hand = setHand;
        handPosition = hand.transform.position;
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        
    }

    public void MoveWithHand()
    {
        transform.position = handPosition;
    }

    public void MoveWithHandNet(Transform handNet)
    {
        transform.position = handNet.position;
    }

    public void rotateGun(float aimAngle) {
        rb.rotation = aimAngle;
        // Rotate gun relative to aimpoint
        if (Mathf.Abs(aimAngle) > 90 && !isFlipped) {
            gameObject.transform.Rotate(180.0f, 0.0f, 0.0f, Space.Self);
            isFlipped = true;
        } else if (Mathf.Abs(aimAngle) < 90 && isFlipped) {
            gameObject.transform.Rotate(180.0f, 0.0f, 0.0f, Space.Self);
            isFlipped = false;
        }

        // Change the order in layer of gun and bullets
        if (aimAngle > 22.5 && aimAngle < 157.5) {
            gun.GetComponent<SpriteRenderer>().sortingOrder = 9;
            bullet.GetComponent<SpriteRenderer>().sortingOrder = 8;
            muzzle1.GetComponent<SpriteRenderer>().sortingOrder = 8;
        } else {
            gun.GetComponent<SpriteRenderer>().sortingOrder = 12;
            bullet.GetComponent<SpriteRenderer>().sortingOrder = 11;
            muzzle1.GetComponent<SpriteRenderer>().sortingOrder = 11;
        }
    }

    // Move Aim Point
    public float followAimPoint() {
        // Get vector relative to gun and mouse position in camera
        Vector2 aim = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim *= -0.5f;

        // Get angle of the aim vector
        float AngleOfAim = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        return AngleOfAim;
    }

}
