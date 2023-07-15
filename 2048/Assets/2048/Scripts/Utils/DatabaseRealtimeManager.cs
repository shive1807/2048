using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class DatabaseRealtimeManager : MonoBehaviour
{
    private DatabaseReference reference;

    private void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        writeNewUser("shiva", "bhati", "shivambhati21@gmail.com");
    }

    private void writeNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
}

public class User
{
    public string username;
    public string email;

    public User()
    {
    }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}