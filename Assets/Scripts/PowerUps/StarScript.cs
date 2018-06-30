using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    [SerializeField]
    private float speed, rotationSpeed;

    private bool started;
    private Rigidbody rb;
    private Collider col;
    private GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (started)
        {
            rb.velocity = new Vector3(-1, -1, 0) * speed;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && started)
        {
            col.gameObject.GetComponentInParent<PlayerState>().GotStar();
            GM.playPowerUp();
            gameObject.SetActive(false);
        }
    }

    public void GetStarted(float time)
    {
        StartCoroutine(AddToPlayer(time));
    }

    IEnumerator AddToPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        rb.useGravity = true;
        col.enabled = true;
        started = true;
    }
}
