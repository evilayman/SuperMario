using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float offset, time;
	
	void LateUpdate ()
    {
        SetPosition();
	}

    private void SetPosition()
    {
        var targetX = target.transform.position.x + offset;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        if(targetX < transform.position.x)
            transform.position = Vector3.Lerp(transform.position, targetPosition, time);
    }

    public void ResetPosition()
    {
        var targetX = target.transform.position.x + offset;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = targetPosition;
    }
}
