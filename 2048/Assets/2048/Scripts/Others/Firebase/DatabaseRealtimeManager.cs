using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DatabaseRealtimeManager : Singleton<DatabaseRealtimeManager>
{
    private DatabaseReference databaseReference;

    public string UserID;
    public User UserData;

    private void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            // Check for any initialization errors
            if (task.Exception != null)
            {
                Debug.LogError($"Firebase Initialization Error: {task.Exception}");
                return;
            }

            // unique user id for every device
            //UserID = SystemInfo.deviceUniqueIdentifier;

            UserID = GameManager.Instance.gameData.User.UserID;

            // Set up the database reference
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            //// Loading user data
            //ReadDataFromFirebase();  // needs user id
            UserDataSetup(UserID);

        });
    }

    public void CreateNewUser()
    {
        // Create a new data entry
        User playerData = new User(UserData.GameData, UserData.Username, UserData.Email);

        // Convert the data to JSON format
        string json = JsonUtility.ToJson(playerData);

        // Push the data to the database (creates a new child node with a unique key)
        databaseReference.Child("Users").Child(UserID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data written to Firebase!");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Error writing data to Firebase: {task.Exception}");
            }
        });
    }

    public void UserDataSetup(string userId)
    {
        if (databaseReference != null)
        {
            DatabaseReference userReference = databaseReference.Child("Users").Child(userId);

            userReference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Error checking user existence: {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.Key);

                    if (snapshot.Exists)
                    {
                        Debug.Log($"User with ID {userId} exists!");
                    }
                    else
                    {
                        Debug.Log($"User with ID {userId} does not exist.");
                        CreateNewUser();
                    }
                }
            });
        }
    }

    public void ReadDataFromFirebase(string userId = default)
    {
        if (databaseReference != null)
        {
            // Create a reference to the user's data based on their unique ID
            DatabaseReference userReference = databaseReference.Child("Users").Child(userId);

            // Read the data from the database
            userReference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        // Deserialize the JSON data into a User object
                        User userData = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());

                        UserData = userData;
                    }
                    else
                    {
                        Debug.LogWarning("User data not found in the database.");
                        CreateNewUser();
                    }
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"Error reading data from Firebase: {task.Exception}");
                }
            });
        }
    }

    public void UpdateUserData(User newData)
    {
        if (databaseReference == null)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        // Create a reference to the user's data node
        databaseReference = databaseReference.Child("Users").Child(UserID);

        // Update the data
        databaseReference.SetValueAsync(newData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating user data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("User data updated successfully.");
            }
        });
    }
}
