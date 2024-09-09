using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput
{
    public Vector2 InputDelta;
    public KeyboardInput()
    {

    }
    public void CustomUpdate()
    {
        InputDelta = Vector2.zero;

        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            InputDelta += Vector2.left;
        }

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            InputDelta += Vector2.right;
        }
    }
}
