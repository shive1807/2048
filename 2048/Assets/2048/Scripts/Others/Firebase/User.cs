[System.Serializable]
public class User
{
    public string UserID;
    public string Username;
    public string Email;

    public User(string name = default, string email = default, string userID = null)
    {
        this.Username = name;
        this.Email = email;
        this.UserID = userID;
    }
}
