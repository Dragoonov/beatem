using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCustomHandler : MonoBehaviour
{
	Firebase.Auth.FirebaseAuth auth;
    GameObject formPanel;
    private bool signInMode;
    public UserManager userManager;
    GameObject userName;
    GameObject signInButton;
    GameObject signUpButton;
    GameObject signOutButton;
    InputField email;
    InputField password;
    Text errorMessage;
    Button multiplayerButton;
    private string errorMessageString;

    public void Start()
    {
        multiplayerButton = GameObject.Find("MultiplayerButton").GetComponent<Button>();
        formPanel = GameObject.Find("FormPanel");
        errorMessage = GameObject.Find("ErrorMessage").GetComponent<Text>();
        email = GameObject.Find("Email").GetComponent<InputField>();
        signOutButton = GameObject.Find("SignOutButton");
        signUpButton = GameObject.Find("SignUpButton");
        signInButton = GameObject.Find("SignInButton");
        password = GameObject.Find("Password").GetComponent<InputField>();
        userName = GameObject.Find("Username");
        userManager = GameObject.Find("User").GetComponent<UserManager>();
        InitializeFirebase();
        ClosePanel();
        if (userManager.GetLoggedUser()!=null)
        {
            ChangeLayoutSignedIn();

        }
        else
        {
            ChangeLayoutSignedOut();
        }
    }

    public void Update()
    {
        if(errorMessageString != null && !errorMessageString.Equals(""))
        {
            errorMessage.text = errorMessageString;
        }
    }

    private void ChangeLayoutSignedIn()
    {
        multiplayerButton.interactable = true;
        userName.SetActive(true);
        userName.GetComponent<Text>().text = userManager.GetLoggedUser().DisplayName;
        signInButton.SetActive(false);
        signUpButton.SetActive(false);
        signOutButton.SetActive(true);
    }

    private void ChangeLayoutSignedOut()
    {
        multiplayerButton.interactable = false;
        userName.SetActive(false);
        signInButton.SetActive(true);
        signUpButton.SetActive(true);
        signOutButton.SetActive(false);
    }
    // Start is called before the first frame update
    public void OnSignIn()
	{
        signInMode = true;
        formPanel.SetActive(true);
    }

    public void OnSignUp()
    {
        signInMode = false;
        formPanel.SetActive(true);
    }

    public void OnSignOut()
    {
        auth.SignOut();
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseUser user = userManager.GetLoggedUser();

        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                userManager.setLoggedUser(null);
                ChangeLayoutSignedOut();
                ClosePanel();
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                userManager.setLoggedUser(user);
                ChangeLayoutSignedIn();
                ClosePanel();
            }
        }
    }

    private void ClosePanel()
    {
        formPanel.SetActive(false);
        errorMessage.text = "";
        errorMessageString = "";
        email.text = "";
        password.text = "";
    }

    public void OnOK()
    {
        string emailString = email.text;
        string passwordString = password.text;
        if (signInMode)
        {
            auth.SignInWithEmailAndPasswordAsync(emailString, passwordString).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    errorMessageString = "Task canceled";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    errorMessageString = task.Exception.GetBaseException().Message;
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                //transform.gameObject.GetComponent<UserManager>().setLoggedUser(newUser);
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        } 
        else
        {
            auth.CreateUserWithEmailAndPasswordAsync(emailString, passwordString).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    errorMessageString = "Task canceled";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    errorMessageString = task.Exception.GetBaseException().Message;
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                //transform.gameObject.GetComponent<UserManager>().setLoggedUser(newUser);
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }
    }

    public void OnBack()
    {
        ClosePanel();
    }

    public void OnExit()
	{
		Application.Quit();
	}


}
