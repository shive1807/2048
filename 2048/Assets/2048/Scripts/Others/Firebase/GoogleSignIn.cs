using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
using System.Threading.Tasks;

public class FirebaseUserAuth : MonoBehaviour
{
    private void Awake()
    {
        //InitializeFirebase();
        CheckDependencies();

    }

    public async Task InitializeFirebase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase correctly Initialized");
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    private void CheckDependencies()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase correctly Initialized");
                AnonymousLogin();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    private void AnonymousLogin()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;



        //User loggin in for the first time.
        if (GameManager.Instance.gameData.FirstLogin == 0)
        {
            auth.SignOut();

            auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result.User;
                LoginSuccessful(newUser.UserId);
            });
        }
        else
        {
            string userID = GameManager.Instance.gameData.User.UserID;
            GetData();
        }
    }

    private void LoginSuccessful(string uid)
    {
        User user = new User();

        string userName = "Guest" + Random.Range(11111, 99999).ToString();
        user.Username = userName;
        user.UserID = uid;

        //set user in database.
        //DataManager.myUser = user;

        //login successful.
        SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, 0, user);

        //write a new user and destroy this object.
        //GetComponent<FirebaseLeaderboardManager>().WriteNewUser();

        Destroy(this);
    }

    private void GetData()
    {
        User user = new User();

        Debug.Log("my player id is " + GameManager.Instance.gameData.User.UserID);

        //user.Username = PlayerPrefs.GetString(PlayerPrefsConstants.userName);
        //user.UserID = PlayerPrefs.GetString(PlayerPrefsConstants.userId);

        //set the user in database.
        //DataManager.myUser = user;

        Destroy(this);
    }
}