using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource themeSound;
    [SerializeField] private AudioSource runSound;
    public static AudioSource Source { get; private set; }
    public static AudioSource Source2 { get; private set; }
    public static AudioSource Source3 { get; private set; }
    public static int score;
    int dolar = 0;
    int baseScor = 5;
    int baseScor2 = 5;
    int batarya = 50;
    public static GameManager inst;

    [SerializeField] Text scoreText;
    [SerializeField] Text dolarText;
    [SerializeField] Text dolar2Text;
    [SerializeField] PlayerMovement playerMovement;

    public void IncrementScore()
    {
        score++;
        scoreText.text = " " + score;

        // Increase the player's speed
        playerMovement.speed += playerMovement.speedIncreasePerPoint;

        IncrementBattery();
        IncrementDolar();


    }

    public void IncrementBattery()
    {
        if (score > baseScor)
        {
            batarya--;
            dolarText.text = " " + batarya + "%";
            baseScor = baseScor * 2;

        }
    }

    public void IncrementDolar()
    {
        if (score > baseScor2)
        {
            dolar++;
            // FUTURE WORK - BURAYA IF KOY (DOLAR 1 DEN FAZLA OLCAK) 1 DOLAR = 18.58 -> 11.10.2022
            dolar2Text.text =  "$" + "0." + dolar ; 
            baseScor2 = baseScor2 * 2;

        }
    }

    public void StartSong()
    {
        coinSound.Play();
    }

    public void StartRunSong()
    {
        coinSound.Play();
    }

    public void StartTheme()
    {
        runSound.Play();
    }

    private void Awake()
    {
        inst = this;
        Source = coinSound;
        Source2 = themeSound;
        Source3 = runSound;
    }


    void EndGame()
    {
        Debug.Log("Game Over");
    }
    // Başlangıçtaki oyunun themeni çalıştırcak
    private void Start()
    {
        GameManager.Source2.Play();
        GameManager.Source3.Play();
    }

    private void Update()
    {

    }
}