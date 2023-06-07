using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public bool pressed;
    [HideInInspector] public bool released;
    [HideInInspector] public Vector2 mousePos;

    void Update()
    {
        MouseCheck();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
