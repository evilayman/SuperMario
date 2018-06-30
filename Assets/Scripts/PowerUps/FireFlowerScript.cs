using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerScript : MonoBehaviour
{
    private GameManager GM;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GM.playPowerUp();
            col.gameObject.GetComponentInParent<PlayerState>().Fireflower = true;
            gameObject.SetActive(false);
        }
    }

}
