using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector]public bool pressed;
    [HideInInspector]public bool released;

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
