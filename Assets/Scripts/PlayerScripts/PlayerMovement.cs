using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpSpeed, gravity;
    private Rigidbody RB;
    private float leftSpeed, rightSpeed;
    private PlayerRay myRays;

    private GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        RB = GetComponent<Rigidbody>();
        myRays = GetComponent<PlayerRay>();
    }

    private void Update()
    {
        leftSpeed = (Input.GetKey(KeyCode.A)) ? speed : 0;
        rightSpeed = (Input.GetKey(KeyCode.D)) ? -speed : 0;
        RestrictPlayer();
    }

    private void RestrictPlayer()
    {
        var playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (playerScreenPos.x <= 0)
        {
            RB.velocity = Vector3.zero;
            leftSpeed = 0;
        }
    }

    void FixedUpdate()
    {
        RB.AddForce(new Vector3(leftSpeed + rightSpeed, 0, 0));

        var onGround = myRays.IsOnGround();

        if (Input.GetKeyDown(KeyCode.W) && onGround)
        {
            GM.playJump();
            RB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
        else if (!onGround)
        {
            RB.AddForce(-Vector3.up * gravity);
        }
    }
}
