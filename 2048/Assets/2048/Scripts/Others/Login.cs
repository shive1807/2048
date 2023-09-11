using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField userInput;
    public User user;
    void Start()
    {

    }

    public void OnNewLogin()
    {
        if (userInput.text == string.Empty)
        {
            userInput.placeholder.GetComponent<TextMeshProUGUI>().text = new string("Please Enter Your Name First");
            userInput.placeholder.color = Color.red;
            return;
        }

        user = new User();
        //user.username = userInput.text;
    }

    public void OnClearInput()
    {
        userInput.text = string.Empty;
    }
}
