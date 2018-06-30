using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float moveDistance, speed, stopMovementTime, InvunrableTime, rayDistance;
    [SerializeField]
    private bool twoHits;

    private Rigidbody rb;
    private Vector3 moveTo, moveFrom, dir, currentTarget;
    private bool canMove = true, canHit = true, died;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        moveFrom = transform.position;
        moveTo = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);

        currentTarget = moveFrom;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = dir * speed;
            CheckPosition();
            HitRay();
        }
    }

    private void CheckPosition()
    {
        if (Vector3.Distance(transform.position, currentTarget) <= 0.2f)
        {
            SwitchDirection();
        }
    }

    private void SwitchDirection()
    {
        if (currentTarget == moveFrom)
        {
            currentTarget = moveTo;
            dir = (moveTo - moveFrom).normalized;
        }
        else
        {
            currentTarget = moveFrom;
            dir = (moveFrom - moveTo).normalized;
        }

        if (transform.position.x > currentTarget.x)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void HitRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, rayDistance))
        {
            if (hit.transform.tag == "Block" || hit.transform.tag == "Pipe" || hit.transform.tag == "Ground")
            {
                SwitchDirection();
            }
            else if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<PlayerState>().IsHit(gameObject);
            }
        }

        if (Physics.Raycast(transform.position, -dir, out hit, rayDistance))
        {
            if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<PlayerState>().IsHit(gameObject);
            }
        }

    }

    public void IsHit()
    {
        if (canHit)
        {
            if (!twoHits)
            {
                Die();
            }
            else
            {
                Slide();
            }
            StartCoroutine(IsInvunrable());
        }

    }

    IEnumerator IsInvunrable()
    {
        canHit = false;
        yield return new WaitForSeconds(InvunrableTime);
        canHit = true;
    }

    public void Die()
    {
        died = true;
        canMove = false;
        rb.velocity = Vector3.zero;
        if (transform.localScale.y > 0)
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        DisableColliders();
        Destroy(gameObject, 5f);
    }

    public void Slide()
    {
        StartCoroutine(StopMovement());
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.left * 10, ForceMode.Impulse);
    }

    private void DisableColliders()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (var collider in cols)
        {
            collider.enabled = false;
        }
    }

    IEnumerator StopMovement()
    {
        twoHits = false;
        canMove = false;
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

        yield return new WaitForSeconds(stopMovementTime);

        if (!died)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            canMove = true;
            twoHits = true;
        }
    }
}
