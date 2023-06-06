using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Linq;
public class Firebasemanager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public static FirebaseUser User;
    public DatabaseReference DBreference;
    public static int tem ;
    public static int tem2 ;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;


     //User Data variables
    [Header("UserData")]
    // public TMP_InputField usernameField;
    // public TMP_InputField xpField;
    public GameObject scoreElement;
    public Transform scoreboardContent;
    public Scoreelement scorelement1;

    public void nextBtn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void  SignOutButton()
    {
        auth.SignOut();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
        SceneManager.LoadScene("Loginoutscene");
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }

    //Function for the save button
    public void SaveDataButton()
    {
        // StartCoroutine(UpdateUsernameAuth(usernameField.text));
        // StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        // StartCoroutine(UpdateXp(int.Parse(xpField.text)));
        // StartCoroutine(UpdateKills(int.Parse(killsField.text)));
        // StartCoroutine(UpdateDeaths(int.Parse(deathsField.text)));
    }

     //Function for the scoreboard button
    public void ScoreboardButton()
    {        
        StartCoroutine(LoadScoreboardData());
    }


    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            tem = 0;
            Debug.Log("User id:  " + User.UserId);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        StartCoroutine(UpdateUsernameAuth(usernameRegisterField.text));
                        StartCoroutine(UpdateUsernameDatabase(usernameRegisterField.text, emailRegisterField.text));
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }        
    }

    private IEnumerator UpdateUsernameDatabase(string _username, string _email)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
        DBTask = DBreference.Child("users").Child(User.UserId).Child("email").SetValueAsync(_email);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    public IEnumerator UpdateHighScoreDatabase(int _xp, FirebaseUser User)
    {
        //Set the currently logged in user xp
         Debug.Log("Icıne girdiiiii ");
        Debug.Log("User id:  " + User.UserId);
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("highscore").SetValueAsync(_xp);
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    public IEnumerator LoadUserData(Text scoreText, FirebaseUser User)
    {
        //Get the currently logged in user data
        
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else if (DBTask.Result.Value == null)
        // {
        //     //No data exists yet
        //     Debug.Log("ICINE GIRDIIIIIIIIII1");
        //     scoreText.text = "0";
           
        // }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log("ICINE GIRDIIIIIIIIII2");
            scoreText.text = snapshot.Child("highscore").Value.ToString();
            tem = int.Parse(snapshot.Child("highscore").Value.ToString());
            Debug.Log(tem);
            
        }
    }

    // public void UpdateHighData2(int _xp, FirebaseUser User)
    // {
    //     Set the currently logged in user xp
    //      Debug.Log(_xp);
    //      Debug.Log(User.UserId);
    //      Debug.Log(DBreference);
    // }

    public IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        var DBTask = DBreference.Child("users").OrderByChild("highscore").GetValueAsync();
        int xp = 0;
        string tempuser;
        GameObject scoreboardElement = null;
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            //   foreach (Transform child in scoreboardContent.transform)
            //   {
            //       Destroy(child.gameObject);
            //   }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                string emailr = childSnapshot.Child("email").Value.ToString();
                
                if(childSnapshot.Child("highscore").Value.ToString() != null) {
                     xp = int.Parse(childSnapshot.Child("highscore").Value.ToString());
                }
                //string xp = childSnapshot.Child("highscore").Value.ToString();
                //Debug.Log(username);
                //Debug.Log(emailr);
                //Debug.Log(xp);

                //Debug.Log("Hata olmadii");
                // //Instantiate new scoreboard elements
                scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                 //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, xp);
                 Debug.Log(scoreboardElement);
                 if(scoreboardElement != null) {
                     //Debug.Log("Buraya girmedi");
                     scorelement1 = GameObject.FindObjectOfType<Scoreelement>();
                     //Debug.Log(scoreboardElement.GetComponent<Scoreelement>());
                    scorelement1.NewScoreElement(username, xp);
                     //scoreboardElement?.GetComponent<ScoreElement>()?.NewScoreElement(username, xp);
                     //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, xp);
                     //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
                }
            }
            scoreboardElement = Instantiate(scoreElement, scoreboardContent);
            scorelement1 = GameObject.FindObjectOfType<Scoreelement>();
            tempuser = "TOP SCORE";
            xp = 10;
            scorelement1.NewScoreElement(tempuser, xp);
            //Go to scoareboard screen
            //SceneManager.LoadScene("ScoreBoardScene");
        }
    }
}
