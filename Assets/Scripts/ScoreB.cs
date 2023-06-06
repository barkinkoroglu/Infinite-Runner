using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreB : MonoBehaviour
{
    public Firebasemanager firebasemanager4;
    
    void Start()
    {
        firebasemanager4 = GameObject.FindObjectOfType<Firebasemanager>();
        StartCoroutine(firebasemanager4.LoadScoreboardData());

        
    }


    public void LogouttoHome()
    {
        firebasemanager4 = GameObject.FindObjectOfType<Firebasemanager>();
        firebasemanager4.SignOutButton();

        
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
