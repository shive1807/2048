using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
using System.Threading.Tasks;
using TMPro;
using System.Collections;

public class AnonymousLogin : MonoBehaviour
{
    public TMP_InputField userInput;

    public bool login = false;

    [HideInInspector] public string Username;
    private void Start()
    {
        InitializeFirebase();
    }

    public void InitializeFirebase()
    {
        //User loggin in for the first time.
        if (GameManager.Instance.gameData.FirstLogin == 1)
        {
            UiManager.Instance.SetActive(transform.GetComponent<RectTransform>(), true);
        }
        else
        {
            string userID = GameManager.Instance.gameData.User.UserID;
            GetData();
        }
    }

    private void CheckDependencies()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                Login();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    private void Login()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

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

            //creating new user
            User user = new User();

            user.UserID = newUser.UserId;
            user.Username = Username;

            //login successful.
            SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, 0, user);
        });
    }

    public IEnumerator RandomUsername()
    {
        string userName;
        userName = "Guest" + Random.Range(11111, 99999).ToString();
        Username = userName;
        login = true;

        CheckDependencies();

        yield return new WaitForSeconds(.2f);

        // UI
        UiManager.Instance.SetActive(transform.GetComponent<RectTransform>(), false);
    }

    public IEnumerator UsernameInput()
    {
        userInput = transform.GetChild(4).GetComponent<TMP_InputField>();

        if (userInput.text == string.Empty)
        {
            userInput.placeholder.GetComponent<TextMeshProUGUI>().text = new string("Please Enter Your Name First");
            userInput.placeholder.color = Color.red;
            yield return null;
        }
        Username = userInput.text;
        login = true;

        CheckDependencies();

        yield return new WaitForSeconds(.2f);

        // UI
        UiManager.Instance.SetActive(transform.GetComponent<RectTransform>(), false);
    }

    public void OnClearInput()
    {
        userInput.text = string.Empty;
    }

    private void GetData()
    {
        User user = new User();

        Debug.Log("my player id is " + GameManager.Instance.gameData.User.UserID);

        //Destroy(this);
    }
}