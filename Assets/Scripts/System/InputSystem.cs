using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField] private float _keyboardSensetivity = 1f;
    [SerializeField] private float _mouseSensetivity = 1f;

    private MouseInput _mouseInput;
    private KeyboardInput _keyboardInput;

    private float _currentInputX = 0f;

    public float CurrentInputX
    {
        get { return _currentInputX; }
    }
    public bool IsMouseClicked => _mouseInput.IsMouseClicked;
    public Vector2 MouseWorldPosition => _mouseInput.MouseWorldPosition;

    public void Init()
    {
        _mouseInput = new MouseInput();
        _keyboardInput = new KeyboardInput();
    }

    public void CustomUpdate()
    {
        _mouseInput.CustomUpdate();
        _keyboardInput.CustomUpdate();

        _currentInputX = _keyboardInput.InputDelta.x * _keyboardSensetivity + _mouseInput.InputDelta.x * _mouseSensetivity;
    }
}
