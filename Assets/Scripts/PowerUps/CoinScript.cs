using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    private GameObject targetPlayer;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !targetPlayer)
        {
            other.gameObject.GetComponentInParent<PlayerState>().AddCoin();
            gameObject.SetActive(false);
        }
    }

    public void GetStarted(GameObject player, float time)
    {
        targetPlayer = player;
        StartCoroutine(AddToPlayer(time));
    }

    IEnumerator AddToPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        targetPlayer.GetComponent<PlayerState>().AddCoin();
        gameObject.SetActive(false);
    }

}
