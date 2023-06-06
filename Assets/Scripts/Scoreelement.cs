using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreelement : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text xpText;

    public void NewScoreElement (string _username, int _xp)
    {
        Debug.Log(_username);
        Debug.Log(_xp);
        usernameText.text = _username;
        xpText.text = _xp.ToString();
    }

}

