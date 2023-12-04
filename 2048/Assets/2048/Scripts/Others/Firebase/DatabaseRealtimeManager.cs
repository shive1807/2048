using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;

public class DatabaseRealtimeManager : Singleton<DatabaseRealtimeManager>
{
    private DatabaseReference databaseReference;

    public string UserID;
    public GameData data = new GameData();
    public long userCount = 0;

    private void OnEnable()
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

            UserID = GameManager.Instance.gameData.User.UserID;

            // Set up the database reference
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void WriteData()
    {
        if (GameManager.Instance.gameData.FirstLogin == 1)
        {
            return;
        }

        if (databaseReference == null)
        {
            Debug.LogWarning("Firebase is not initialized.");
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        // Convert the data to JSON format
        string json = JsonUtility.ToJson(GameManager.Instance.gameData);

        // Push the data to the database (creates a new child node with a unique key)
        databaseReference.Child("Users").Child(GameManager.Instance.gameData.User.UserID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
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

    //public void UserDataSetup(string userId)
    //{
    //    if (databaseReference != null)
    //    {
    //        DatabaseReference userReference = databaseReference.Child("Users").Child(userId);

    //        userReference.GetValueAsync().ContinueWithOnMainThread(task =>
    //        {
    //            if (task.IsFaulted)
    //            {
    //                Debug.LogError($"Error checking user existence: {task.Exception}");
    //            }
    //            else if (task.IsCompleted)
    //            {
    //                DataSnapshot snapshot = task.Result;
    //                Debug.Log(snapshot.Key);

    //                if (snapshot.Exists)
    //                {
    //                    Debug.Log($"User with ID {userId} exists!");
    //                }
    //                else
    //                {
    //                    Debug.Log($"User with ID {userId} does not exist.");
    //                    CreateNewUser();
    //                }
    //            }
    //        });
    //    }
    //}

    public async Task<GameData> RetrieveData(string userId = default)
    {
        GameData data = new GameData();
        if (databaseReference != null)
        {
            // Create a reference to the user's data based on their unique ID
            DatabaseReference userReference = databaseReference.Child("Users").Child(userId);

            try
            {
                // Read the data from the database asynchronously
                DataSnapshot snapshot = await userReference.GetValueAsync();

                if (snapshot.Exists)
                {
                    // Deserialize the JSON data into a User object
                    data = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());
                }
                else
                {
                    Debug.LogWarning("User data not found in the database.");
                    WriteData();

                    data = GameManager.Instance.gameData;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading data from Firebase: {ex}");
            }
        }
        return data;
    }


    public async Task<GameData[]> FetchCurrentUserRanks()
    {
        GameData[] users = new GameData[Leaderboard.Instance.leaderboardSize];
        GameData currentUser = GameManager.Instance.gameData;

        users[0] = currentUser;

        if (databaseReference != null)
        {
            try
            {
                DataSnapshot snapshot = await databaseReference.Child("Users").OrderByChild("HighScore").GetValueAsync();
                userCount = snapshot.ChildrenCount;
                int rank = 0;

                for (int i = 1; i < users.Length; i++)
                {
                    int index = ((i - 1) < 0) ? 0 : (i - 1);

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        int userHighscore = int.Parse(userSnapshot.Child("HighScore").Value.ToString());
                        string userId = userSnapshot.Child("User").Child("UserID").Value.ToString();

                        if (users[index] == null)
                        {
                            i = users.Length;
                            break;
                        }

                        else if (users[index].HighScore < userHighscore)
                        {
                            rank++;

                            if (users[i] == null)
                            {
                                users[i] = await RetrieveData(userId);
                            }

                            else if (users[i].HighScore >= userHighscore)
                            {
                                users[i] = await RetrieveData(userId);
                            }
                        }
                    }
                }
                
                int filledSpace = users.Count(i => i != null);
                int emptySpace = users.Length - filledSpace;

                if (emptySpace > 0 )
                {
                    GameData[] temp = new GameData[users.Length];

                    // moving the elements to upward in array
                    for (int i = 0; i < filledSpace; i++)
                    {
                        temp[users.Length - filledSpace + i] = users[i];
                    }

                    users = temp;

                    for (int i = emptySpace - 1; i >= 0; i--)
                    {
                        int index = ((i + 1) < 0) ? 0 : (i + 1);

                        foreach (DataSnapshot userSnapshot in snapshot.Children)
                        {
                            int userHighscore = int.Parse(userSnapshot.Child("HighScore").Value.ToString());
                            string userId = userSnapshot.Child("User").Child("UserID").Value.ToString();

                            if (users[index].HighScore > userHighscore)
                            {
                                if (users[i] == null)
                                {
                                    users[i] = await RetrieveData(userId);
                                }

                                else if (users[i].HighScore <= userHighscore)
                                {
                                    users[i] = await RetrieveData(userId);
                                }
                            }
                        }
                    }

                }

                Leaderboard.Instance.rank = rank;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error fetching user rank: {e}");
            }
        }
        return users;
    }


    public void UserNameCheck(string username)
    {

    }
}
