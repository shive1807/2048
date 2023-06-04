using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool pressed;
    public bool released;

    void Update()
    {
        MouseCheck();
    }

    void MouseCheck()
    {
        if(Input.GetMouseButton(0))
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            released = true;
        }
    }
}
