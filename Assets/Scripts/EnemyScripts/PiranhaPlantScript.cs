using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlantScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerState>().IsHit(gameObject);
        }
    }
}
