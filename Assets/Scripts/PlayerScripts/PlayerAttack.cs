using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    private PlayerState pS;
    private GameManager GM;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        pS = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pS.Fireflower)
        {
            var bulletPos = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
            GM.playFireBall();
            Instantiate(bullet, bulletPos, Quaternion.identity);
        }
    }
}
