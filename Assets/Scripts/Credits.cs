using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
public class Credits : MonoBehaviour
{
    [SerializeField] private AudioSource creditSound;
    public Firebasemanager firebasemanager3;
    
    public static AudioSource Source { get; private set; }
    public Text scoreText;
    public static int  tempValue;
    public static Credits inst;
     public void Start()
    {
        
        Credits.Source.Play();
        int temp = GameManager.score;
        firebasemanager3 = GameObject.FindObjectOfType<Firebasemanager>();
        //scoreText.text = PlayerPrefs.GetInt("HighScoree",0).ToString();
        // firebasemanager3.UpdateHighScoreDatabase(int.Parse(scoreText.text));
        //firebasemanager3.UpdateHighData2(PlayerPrefs.GetInt("HighScoree",0),Firebasemanager.User);
        //firebasemanager3.UpdateHighScoreDatabase(PlayerPrefs.GetInt("HighScoree",0),Firebasemanager.User);
        StartCoroutine(firebasemanager3.LoadUserData(scoreText ,Firebasemanager.User));
        //Debug.Log("DATAAAAAAAA");
        // Debug.Log(int.Parse(scoreText.text));
        // Debug.Log(scoreText.text);
        
        Invoke("LaunchProjectile", 1.0f);
        
        // Firebasemanager.tem = 0;
    }
    
    public void Quit ()
    {
        
        Application.Quit();
        
    }

     public void LaunchProjectile ()
    {
        Debug.Log("(GameManager.score) degeri:" + GameManager.score);
        Debug.Log("(Firebasemanager.tem) degeri:" + Firebasemanager.tem);
        if(GameManager.score > Firebasemanager.tem)
        {
            //PlayerPrefs.SetInt("HighScoree", GameManager.score);
            StartCoroutine(firebasemanager3.UpdateHighScoreDatabase(GameManager.score ,Firebasemanager.User));
            Firebasemanager.tem = GameManager.score;
            //Debug.Log("DEGERI ASTIIIIII");
            //scoreText.text = GameManager.score.ToString();
        }
       // StartCoroutine(firebasemanager3.UpdateHighScoreDatabase(PlayerPrefs.GetInt("HighScoree",0),Firebasemanager.User));
       StartCoroutine(firebasemanager3.LoadUserData(scoreText ,Firebasemanager.User));
        
        GameManager.score = 0;
        
        
    }

    public void Reset()
    {

        PlayerPrefs.DeleteKey("HighScoree");
        int delete = 0;
        scoreText.text = " " + delete;

    }

    public void TryAgain()
    {
        SceneManager.LoadScene("SampleScene");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);

    }

    public void StartCreditSong()
    {
        creditSound.Play();
    }


    private void Awake()
    {
        inst = this;
        Source = creditSound;
        
    }

    

    
}
