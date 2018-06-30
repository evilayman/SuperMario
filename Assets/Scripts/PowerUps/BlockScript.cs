using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public enum PowerType
    {
        None,
        Coin,
        Mushroom,
        FireFlower,
        Star
    }

    [Header("Block Content")]
    [SerializeField]
    private PowerType powerType;

    [Header("Block Type")]
    [SerializeField]
    private bool isDestructuble;

    [Header("Block Mats")]
    [SerializeField]
    private Material NormalBlockMat;
    [SerializeField]
    private Material PrizeBlockMat, PrizeBlockHitMat, DestructubleBlockMat;

    [Header("Block Powers Prefabs")]
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject mushroom, fireFlower, star;

    [Header("BlockSettings")]
    [SerializeField]
    private float min;
    [SerializeField]
    private float max, spwanToYPosition, lerpDuration, lerpTime, numberOfCoins;

    private Material myMat;
    private float t;
    private bool hit, reachedPeak;
    private Vector3 spwanTo;
    private GameObject go;
    private GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetMat();
        spwanTo = new Vector3(transform.position.x, transform.position.y + spwanToYPosition, transform.position.z);
    }

    private void SetMat()
    {
        if (isDestructuble)
            GetComponent<Renderer>().material = DestructubleBlockMat;
        else if (powerType == PowerType.None)
            GetComponent<Renderer>().material = NormalBlockMat;
        else
            GetComponent<Renderer>().material = PrizeBlockMat;

        myMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!hit && !isDestructuble && powerType != PowerType.None)
            myMat.color = new Color(myMat.color.r, LerpBetween(ref min, ref max) / 255f, myMat.color.b);

        if (hit && powerType != PowerType.None)
        {
            if (go && go.transform.position.y + 0.1f < spwanTo.y && !reachedPeak)
            {
                go.transform.position = Vector3.Lerp(go.transform.position, spwanTo, lerpTime);
            }
            else if (numberOfCoins > 0 && powerType == PowerType.Coin)
            {
                hit = false;
            }
            else
                reachedPeak = true;
        }
    }

    public void IsHit(GameObject player)
    {
        if (!hit)
        {
            hit = true;

            switch (powerType)
            {
                case PowerType.Coin:
                    go = Instantiate(coin, transform.position, Quaternion.identity);
                    GM.playBump();
                    go.GetComponent<CoinScript>().GetStarted(player, lerpDuration);
                    numberOfCoins--;
                    reachedPeak = false;
                    break;
                case PowerType.Mushroom:
                    go = Instantiate(mushroom, transform.position, Quaternion.identity);
                    GM.playBump();
                    go.GetComponent<MushroomScript>().GetStarted(lerpDuration);
                    break;
                case PowerType.FireFlower:
                    if (player.GetComponent<PlayerState>().Mushroom)
                    {
                        go = Instantiate(fireFlower, transform.position, Quaternion.identity);
                        GM.playBump();
                        GM.playPowerUp();
                    }
                    break;
                case PowerType.Star:
                    go = Instantiate(star, transform.position, Quaternion.identity);
                    GM.playBump();
                    go.GetComponent<StarScript>().GetStarted(lerpDuration);
                    break;
                default:
                    break;
            }

            if (isDestructuble)
            {
                if ((numberOfCoins == 0 && powerType == PowerType.Coin) || powerType != PowerType.Coin)
                    StartCoroutine(Disable());
            }
            else if (powerType != PowerType.None)
            {
                if ((numberOfCoins == 0 && powerType == PowerType.Coin) || powerType != PowerType.Coin)
                    GetComponent<Renderer>().material = PrizeBlockHitMat;
            }
        }
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    float LerpBetween(ref float min, ref float max)
    {
        var value = Mathf.Lerp(min, max, t);

        t += 0.7f * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = max;
            max = min;
            min = temp;
            t = 0.0f;
        }
        return value;
    }
}