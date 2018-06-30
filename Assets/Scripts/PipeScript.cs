using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    [SerializeField]
    private bool exitPipe;
    [SerializeField]
    private Transform targetPipe;

    [SerializeField]
    private GameObject plant;
    [SerializeField]
    private bool hasPlant;

    private float t;
    private Vector3 startPosition, endPosition, plantStart, plantEnd, targetPosition, dir;
    private Transform targetPlayer;
    private bool inPipe;
    private GameObject myPlant;
    private GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        plantStart = startPosition = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y + 2, transform.GetChild(0).position.z);
        plantEnd = endPosition = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y - 2, transform.GetChild(0).position.z);

        plantStart = startPosition - new Vector3(0, 1.5f, 0);
        plantEnd = endPosition - new Vector3(0, 0, 0);

        if (hasPlant)
            myPlant = Instantiate(plant, plantEnd, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !exitPipe)
        {
            targetPlayer = other.transform.parent.transform;
        }
    }

    void Update()
    {
        if (targetPlayer && Input.GetKeyDown(KeyCode.S))
        {
            GM.playPipe();
            inPipe = true;
            targetPlayer.GetComponent<Rigidbody>().isKinematic = true;
            targetPosition = endPosition;
            StartCoroutine(MoveToNextPipe());
        }

        if (inPipe)
        {
            dir = (targetPosition - targetPlayer.position).normalized;
            targetPlayer.Translate(dir * 2 * Time.deltaTime);
        }

        if (hasPlant)
        {
            myPlant.transform.position = LerpBetween(ref plantStart, ref plantEnd);
        }
    }

    IEnumerator MoveToNextPipe()
    {
        yield return new WaitForSeconds(1f);
        targetPipe.GetComponent<PipeScript>().GetPlayer(targetPlayer);
        inPipe = false;
    }

    public void GetPlayer(Transform player)
    {
        targetPlayer = player;
        inPipe = true;
        targetPlayer.transform.position = endPosition;
        targetPosition = startPosition;
        StartCoroutine(FreePlayer());
    }

    IEnumerator FreePlayer()
    {
        yield return new WaitForSeconds(1f);
        targetPlayer.GetComponent<Rigidbody>().isKinematic = false;
        inPipe = false;
    }

    Vector3 LerpBetween(ref Vector3 min, ref Vector3 max)
    {
        var value = Vector3.Lerp(min, max, t);

        t += 0.3f * Time.deltaTime;

        if (t > 1.0f)
        {
            Vector3 temp = max;
            max = min;
            min = temp;
            t = 0.0f;
        }
        return value;
    }
}
