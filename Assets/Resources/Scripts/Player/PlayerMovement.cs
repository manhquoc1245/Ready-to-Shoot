using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovement : NetworkBehaviour {
    // Movement attributes
    private Vector3 moveDir;
    private float moveX;
    private float moveY;

    [SerializeField]
    private GameObject dust;
    private bool canDust;

    [Header("Movement config")]
    [SerializeField]
    [Range(0f, 100f)]
    private float speed;

    private bool isDashing = false;
    [SerializeField]
    private float dashBoost=20f, dashTime=0.1f;
    private float _dashTime;
    [SerializeField]
    private GameObject dashEffect;
    [SerializeField]
    private float dashDelayTime= 0.04f;
    private Coroutine dashEffectCoroutine;
    int iMulti;

    void Start() {
        InvokeRepeating ("PlaySound", 0.0f, UnityEngine.Random.Range(0.25f, 0.45f));
        canDust = true;
        iMulti = PlayerPrefs.GetInt("Multiplayer");

    }

    void Update() {

        if(iMulti == 1)
        {
            if (!IsOwner)
            {
                return;
            }
        }
        
        Movement();
        
    }

    void PlaySound() {
        if (Mathf.Abs(moveX) > 0 || Mathf.Abs(moveY) > 0)
            SoundManager.PlaySound("Steps");
    }

    // Player movement
    void Movement()
    {
        moveX = Input.GetAxis("Horizontal") * speed;
        moveY = Input.GetAxis("Vertical") * speed;
        moveDir = new Vector3(moveX, moveY, 0f);

        Dash();

        if (moveDir.magnitude > 0 && canDust)
            StartCoroutine(WaitToDust());

        gameObject.GetComponent<Rigidbody2D>().velocity = moveDir;

        if (moveX != 0 || moveY != 0)
            gameObject.GetComponent<Animator>().SetBool("isMoving", true);
        else
            gameObject.GetComponent<Animator>().SetBool("isMoving", false);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _dashTime <=0 && isDashing == false) 
        {
            speed += dashBoost;
            _dashTime = dashTime;
            isDashing = true;
            StartDashEffect();
        }
        if(_dashTime <= 0 && isDashing == true)
        {
            speed -= dashBoost;
            isDashing=false;
            StopDashEffect();
        }else _dashTime -= Time.deltaTime;
    }

    void StopDashEffect()
    {
        if(dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
    }

    void StartDashEffect()
    {
        if(dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }

    private IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject effect = Instantiate(dashEffect, transform.position,transform.rotation);
            Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
            effect.GetComponent<SpriteRenderer>().sprite = currentSprite;
            Destroy(effect,0.2f);
            yield return new WaitForSeconds(dashDelayTime);
        }
    }

    private IEnumerator WaitToDust () {
        canDust = !canDust;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.15f, 0.25f));
        canDust = !canDust;
        if (moveDir.magnitude > 0)
        {
            if(iMulti == 0)
                Instantiate(dust, transform.Find("Shadow").transform.position, Quaternion.identity);
            else
                NetworkObjectSpawner.SpawnNewNetworkObject(dust, transform.Find("Shadow").transform.position);
        }
            
    }

}
