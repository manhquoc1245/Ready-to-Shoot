using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class GunController : NetworkBehaviour
{
    private Shoot shoot;
    private Rotate rotate;
    private int iMulti;
    private float aim;
    NetworkObjectPool m_ObjectPool;
    // Start is called before the first frame update
    void Start()
    {
        iMulti = PlayerPrefs.GetInt("Multiplayer");

        
        shoot = GetComponent<Shoot>();

        rotate = GetComponent<Rotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (iMulti == 1)
        {
            
        }
        else
        {
            shoot.ActiveShoot();

            rotate.MoveWithHand();

            float aim = rotate.followAimPoint();

            rotate.rotateGun(aim);

            
        }
        
    }
}
