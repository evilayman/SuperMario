using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField]
    private int numRays;

    private Collider col;

    private bool[] onGroundRays;

    void Start()
    {
        col = GetComponentInChildren<Collider>();
        onGroundRays = new bool[numRays + 1];
    }

    void FixedUpdate()
    {
        CreateRayPositions();
    }

    private void CreateRayPositions()
    {
        float incBy = (col.bounds.max.x - col.bounds.min.x) / numRays;
        float xValue = col.bounds.min.x;
        var distance = ((col.bounds.max.y - col.bounds.min.y) / 2) + 0.1f;

        for (int i = 0; i <= numRays; i++)
        {
            var start = new Vector3(xValue, col.bounds.center.y, col.bounds.center.z);
            onGroundRays[i] = RayCheckGround(start, distance);
            RayCheckUp(start, distance);
            RayCheckEnemy(start, distance + 0.5f);
            xValue += incBy;
        }
    }

    private bool RayCheckGround(Vector3 start, float distance)
    {
        RaycastHit hit;
        //Debug.DrawLine(start, new Vector3(start.x, start.y - distance, start.z), Color.red);
        if (Physics.Raycast(start, Vector3.down, out hit, distance))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Block" || hit.transform.tag == "Pipe")
            {
                return true;
            }
        }
        else
            return false;

        return false;
    }

    public bool IsOnGround()
    {
        for (int i = 0; i < onGroundRays.Length; i++)
        {
            if (onGroundRays[i] == true)
                return true;
        }
        return false;
    }

    void RayCheckUp(Vector3 start, float distance)
    {
        RaycastHit hit;

        if (Physics.Raycast(start, Vector3.up, out hit, distance))
        {
            if (hit.transform.tag == "Block")
            {
                hit.transform.GetComponent<BlockScript>().IsHit(gameObject);
            }
        }
    }

    void RayCheckEnemy(Vector3 start, float distance)
    {
        RaycastHit hit;

        if (Physics.Raycast(start, -Vector3.up, out hit, distance))
        {
            if (hit.transform.tag == "Enemy")
            {
                if (GetComponent<PlayerState>().Star)
                    hit.transform.GetComponent<EnemyScript>().Die();
                else
                    hit.transform.GetComponent<EnemyScript>().IsHit();
            }
        }
    }
}
