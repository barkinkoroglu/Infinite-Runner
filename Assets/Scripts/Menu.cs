using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioSource menuSound;
    public static AudioSource Source { get; private set; }
    public static Menu inst;
    public void StartMenuSong()
    {
        menuSound.Play();
    }

    private void Awake()
    {
        inst = this;
        Source = menuSound;
        
    }
    public void StartGame()
    {
        //GameManager.Source2.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
        
    }

    public void OpenSettings()
    {
        //GameManager.Source2.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        
    }

     public void Start()  {
        Menu.Source.Play();
     }
}
