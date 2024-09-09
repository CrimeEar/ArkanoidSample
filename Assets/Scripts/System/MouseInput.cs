using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput
{
    public Vector2 InputDelta;
    public Vector2 MouseWorldPosition;
    public bool IsMouseClicked;

    private Vector2 _previousMousePosition;
    private Vector2 _screenSize;
    private Camera _mainCamera;

    public MouseInput()
    {
        _screenSize = new Vector2(Screen.width, Screen.height);
        _previousMousePosition = GetNormalizedMousePosition();
        _mainCamera = Camera.main;
    }

    public void CustomUpdate()
    {
        Vector2 currentMousePosition = GetNormalizedMousePosition();

        InputDelta = currentMousePosition - _previousMousePosition;

        _previousMousePosition = currentMousePosition;

        IsMouseClicked = Input.GetMouseButtonDown(0);
        MouseWorldPosition = GetMouseWorldPosition();
    }

    private Vector2 GetNormalizedMousePosition()
    {
        return new Vector2(Input.mousePosition.x / _screenSize.x, Input.mousePosition.y / _screenSize.y);
    }
    private Vector2 GetMouseWorldPosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
