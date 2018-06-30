using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private float playerLife, normalScale, targetScale, starTime;

    [SerializeField]
    private TextMeshProUGUI coinsText, lives, GameOverText;
    private int myCoins;

    [SerializeField]
    private Color ToColor;

    [SerializeField]
    public GameManager GM;

    private bool mushroom, fireflower, star;

    public bool Fireflower
    {
        get
        {
            return fireflower;
        }

        set
        {
            fireflower = value;
        }
    }
    public bool Star
    {
        get
        {
            return star;
        }

        set
        {
            star = value;
        }
    }
    public bool Mushroom
    {
        get
        {
            return mushroom;
        }

        set
        {
            mushroom = value;
        }
    }

    public int MyCoins
    {
        get
        {
            return myCoins;
        }

        set
        {
            myCoins = value;
        }
    }

    private Rigidbody rb;
    private float t;
    private Material myMat;
    private Color FromColor, startColor;
    private bool Died;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        myMat = GetComponentInChildren<Renderer>().material;
        startColor = FromColor = myMat.color;
    }

    private void Update()
    {
        if (Mushroom)
            transform.localScale = LerpTo(transform.localScale, Vector3.one * targetScale);
        else
            transform.localScale = LerpTo(transform.localScale, Vector3.one * normalScale);

        if (star)
            myMat.color = LerpBetween(ref FromColor, ref ToColor);
        else
            myMat.color = startColor;


        if (transform.position.y < -10 && !Died)
        {
            Die();
        }
    }

    public void AddCoin()
    {
        MyCoins++;
        GM.playCoin();
        coinsText.text = "Coins: " + MyCoins.ToString();
    }

    private Vector3 LerpTo(Vector3 from, Vector3 to)
    {
        return Vector3.Lerp(from, to, 0.1f);
    }

    public void IsHit(GameObject enemy)
    {
        if (star && enemy.tag == "Enemy")
        {
            enemy.GetComponent<EnemyScript>().Die();
        }
        else
        {
            Die();
        }
    }

    public void GotStar()
    {
        StartCoroutine(StartStar());
    }

    IEnumerator StartStar()
    {
        Star = true;
        yield return new WaitForSeconds(starTime);
        Star = false;
    }

    public void Die()
    {
        GameOverText.text = "You Died !";

        Fireflower = false;
        Star = false;
        Mushroom = false;

        GM.stop();
        GM.playDie();
        Died = true;
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        DisableColliders();
        StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(3f);

        playerLife--;
        lives.text = "Lives: " + playerLife.ToString();
        if (playerLife == 0)
        {
            GM.GameOver();
        }
        else
        {
            transform.position = new Vector3(0, 0.5f, 0);
            Camera.main.gameObject.GetComponent<PlayerCamera>().ResetPosition();
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            EnableColliders();
            Died = false;
            GameOverText.text = "";
            GM.playTheme();

        }
    }

    private void DisableColliders()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (var collider in cols)
        {
            collider.enabled = false;
        }
    }

    private void EnableColliders()
    {
        var cols = GetComponentsInChildren<Collider>();
        foreach (var collider in cols)
        {
            collider.enabled = true;
        }
    }

    Color LerpBetween(ref Color min, ref Color max)
    {
        var value = Color.Lerp(min, max, t);

        t += 0.9f * Time.deltaTime;

        if (t > 1.0f)
        {
            Color temp = max;
            max = min;
            min = temp;
            t = 0.0f;
        }
        return value;
    }
}
