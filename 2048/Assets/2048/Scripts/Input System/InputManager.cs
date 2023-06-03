using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool pressed;
    public static bool released;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseCheck();
    }

    void mouseCheck()
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
