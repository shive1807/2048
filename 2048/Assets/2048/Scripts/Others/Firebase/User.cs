[System.Serializable]
public class User
{
    public string UserID;
    public string Username;
    public string Email;
    public GameData GameData;

    public User(GameData gameData = default, string name = default, string email = default, string userID = null)
    {
        this.Username = name;
        this.Email = email;
        this.GameData = gameData;
        this.UserID = userID;
    }
}
