using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GoogleSignInManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public TextMeshProUGUI infoText;
    public Button signInButton;
    public Button signOutButton;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready to use.");
                InitializeUI();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    private void InitializeUI()
    {
        signInButton.onClick.AddListener(() => SignInWithGoogle());
        signOutButton.onClick.AddListener(() => SignOut());

        UpdateUI();
    }

    private void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "599511966350-llmh0kh117jradp6hs2dkfs160pij8r5.apps.googleusercontent.com", // Replace with your web client ID
            RequestEmail = true,
        };

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Google Sign-In canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError($"Google Sign-In error: {task.Exception}");
                return;
            }

            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    Debug.LogError("Firebase Sign-In canceled.");
                    return;
                }

                if (authTask.IsFaulted)
                {
                    Debug.LogError($"Firebase Sign-In error: {authTask.Exception}");
                    return;
                }

                user = authTask.Result;
                Debug.Log($"User signed in: {user.DisplayName} ({user.UserId})");

                UpdateUI();
            });
        });
    }

    private void SignOut()
    {
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();

        Debug.Log("User signed out.");

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (user != null)
        {
            signInButton.interactable = false;
            signOutButton.interactable = true;
            infoText.text = $"Welcome, {user.DisplayName}!";
        }
        else
        {
            signInButton.interactable = true;
            signOutButton.interactable = false;
            infoText.text = "Sign in with Google";
        }
    }
}
