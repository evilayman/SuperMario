using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float GameTime, decrementBy;

    [SerializeField]
    private TextMeshProUGUI TimerText, GameOverText;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip ThemeMusic;
    [SerializeField]
    private AudioClip Jump, Coin, Pipe, Die, PowerUp, Won, GameOverSound, Bump, FireBall;

    private CooldownTimer countDown;
    private bool gameOver;
    private AudioSource myPlayer;

    void Start()
    {
        countDown = new CooldownTimer(decrementBy, true);
        myPlayer = GetComponent<AudioSource>();
        playTheme();

    }
    public void playTheme()
    {
        myPlayer.PlayOneShot(ThemeMusic, 0.5f);
    }
    public void stop()
    {
        myPlayer.Stop();
    }
    public void playJump()
    {
        myPlayer.PlayOneShot(Jump, 0.7f);
    }
    public void playCoin()
    {
        myPlayer.PlayOneShot(Coin, 0.7f);
    }
    public void playPipe()
    {
        myPlayer.PlayOneShot(Pipe, 0.7f);
    }
    public void playDie()
    {
        myPlayer.PlayOneShot(Die, 0.7f);
    }
    public void playPowerUp()
    {
        myPlayer.PlayOneShot(PowerUp, 0.7f);
    }
    public void playWon()
    {
        myPlayer.PlayOneShot(Won, 0.7f);
    }
    public void playGameOver()
    {
        myPlayer.PlayOneShot(GameOverSound, 0.7f);
    }
    public void playBump()
    {
        myPlayer.PlayOneShot(Bump, 0.7f);
    }
    public void playFireBall()
    {
        myPlayer.PlayOneShot(FireBall, 0.7f);
    }

    void Update()
    {
        if (countDown.IsReady() && GameTime != 0 && !gameOver)
        {
            countDown.Reset();
            GameTime -= decrementBy;
            TimerText.text = "Time: " + GameTime.ToString();
        }

        if (GameTime == 0)
        {
            player.GetComponent<PlayerState>().Die();
            GameOver();
        }
    }

    public void GameOver()
    {
        stop();
        playGameOver();
        gameOver = true;
        player.GetComponent<Rigidbody>().isKinematic = true;
        GameOverText.text = "Game Over";
    }

    public void GameWon()
    {
        stop();
        playWon();
        gameOver = true;

        player.GetComponent<Rigidbody>().isKinematic = true;

        var coins = player.GetComponent<PlayerState>().MyCoins;
        var time = GameTime;

        GameOverText.text = "You Won !\nCoins Collected: " + coins.ToString() + "\nTime Finished: " + time.ToString();
    }
}
