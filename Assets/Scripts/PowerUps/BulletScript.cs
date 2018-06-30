using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed, reverseTime;

    private Rigidbody rb;
    private Vector3 dir;

    private bool hit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dir = new Vector3(-1, -1, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            StartCoroutine(ChangeDirection());
        }
        else if(collision.gameObject.tag == "Enemy" && !hit)
        {
            hit = true;
            collision.gameObject.GetComponent<EnemyScript>().IsHit();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = (dir * speed);
    }

    private IEnumerator ChangeDirection()
    {
        dir = new Vector3(-1, 1, 0);
        yield return new WaitForSeconds(reverseTime);
        dir = new Vector3(-1, -1, 0);
    }

}
