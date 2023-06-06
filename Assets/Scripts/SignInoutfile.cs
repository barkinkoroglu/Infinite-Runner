using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SignInoutfile : MonoBehaviour
{
    public Firebasemanager firebasemanager2;
     public void BackandLogout()
    {
        //fmanager = GameObject.Find("SignOutButton").GetComponent<FirebaseManager>();
        //firebasemanager2 = GameObject.FindObjectOfType<Firebasemanager>();
        firebasemanager2 = GameObject.FindObjectOfType<Firebasemanager>();
        //Debug.LogFormat("Text: " + firebasemanager2.tem);
        firebasemanager2.SignOutButton();
        

    }


}
