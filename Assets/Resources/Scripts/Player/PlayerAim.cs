using System;
using Unity.Netcode;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerAim : MonoBehaviour {

    [SerializeField]
    private GameObject aimPoint;
    private Rigidbody2D aimRb2D;
    void Start()
    {
    }

    void Update() {

    }

    public void MoveAimPoint() {

        aimPoint.SetActive(true);
        aimRb2D = aimPoint.GetComponent<Rigidbody2D>();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Get vector relative to player and mouse position in camera
        Vector2 aim = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim *= -1f;
        aimPoint.transform.localPosition = aim;

        // Get angle of the aim vector
        AngleOfAim = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
    }

    public float AngleOfAim { get; private set; } = 0;
}
