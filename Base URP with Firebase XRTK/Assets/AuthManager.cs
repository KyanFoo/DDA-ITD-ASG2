using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference mDatabaseRef;

    public TMP_InputField emailRegister;
    public TMP_InputField passwordRegister;
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    public TextMeshProUGUI registerFail;
    public TextMeshProUGUI loginFail;
    public string uid;
    public TMP_InputField usernameRegister;
    public TextMeshProUGUI helloText;
    public string username;
    public int creationTime;
    public string emailAddress;

    private void Awake()
    {
        //sets up firebase
        auth = FirebaseAuth.DefaultInstance;
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void RegisterButton()
    {
        Register(emailRegister.text, passwordRegister.text);
    }
    public void SignInButton()
    {
        SignInUser(emailLogin.text, passwordLogin.text);
    }

    private void Register(string email, string password)
    {
        //registers the user, then creates user data in firebase with their email, username and creation time
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Sorry, there was an error creating your new account, ERROR: " + task.Exception);
                registerFail.gameObject.SetActive(true);
                loginFail.gameObject.SetActive(false);
                return;
            }
            else if (task.IsCompleted)
            {
                Firebase.Auth.FirebaseUser newPlayer = task.Result;
                SceneManager.LoadScene(1);
                Debug.LogFormat("User signed in successfully: ({0})", newPlayer.UserId);
                string uid = newPlayer.UserId;
                var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
                var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
                int creationTime = (int)timestamp;
                WriteNewUser(uid, emailRegister.text, usernameRegister.text, creationTime);
            }
        });
        Firebase.Auth.FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = usernameRegister.text,
            };
            currentUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    public void SignInUser(string email, string password)
    {
        //logs in according to the credentials
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(authTask =>
        {
            if (authTask.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPassword Sync was cancelled");
                return;
            }
            if (authTask.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPassword Async encountered an error: " + authTask.Exception);
                registerFail.gameObject.SetActive(false);
                loginFail.gameObject.SetActive(true);
                return;
            }
            FirebaseUser currentPlayer = authTask.Result;
            if (currentPlayer != null)
            {
                Debug.LogFormat("Welcome to NINJA FOODS {0}", currentPlayer.Email);
                Debug.LogFormat("User ID: {0}", currentPlayer.UserId);
                Debug.LogFormat("User signed in successfully: {0} {1}", currentPlayer.DisplayName, currentPlayer.UserId);
                Debug.LogFormat("Using current user {0}", auth.CurrentUser.Email);
                SceneManager.LoadScene(1);
            }
        });
    }

    public void SignOut()
    {
        //signs current user out
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            SceneManager.LoadScene(0);
        }

    }

    private void WriteNewUser(string userId, string email, string username, int creationDate)
    {
        //writes the new user's id, email, username and creation date into firebase
        User user = new User(email, username, creationDate);
        string json = JsonUtility.ToJson(user);

        mDatabaseRef.Child("User").Child(userId).SetRawJsonValueAsync(json);
    }

    public void ResetPassword()
    {
        Firebase.Auth.FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            emailAddress = currentUser.Email;
        }
        if (currentUser != null)
        {
            auth.SendPasswordResetEmailAsync(emailAddress).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password reset email sent successfully.");
            });
        }
    }
}
