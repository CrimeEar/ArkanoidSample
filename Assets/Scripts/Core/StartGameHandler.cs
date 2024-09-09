using UnityEngine;

public class StartGameHandler
{
    private const int GAME_STATE_INDEX = 1;

    private CircleObject _circleObject;
    private InputSystem _inputSystem;
    public StartGameHandler(CircleObject startCircle, InputSystem inputSystem)
    {
        _circleObject = startCircle;
        _inputSystem = inputSystem;
    }
    public void CustomUpdate()
    {
        if (IsTapOnCircle())
        {
            GameStateMachine.Instance.StartNewState(GAME_STATE_INDEX);
        }
    }

    private bool IsTapOnCircle()
    {
        return _inputSystem.IsMouseClicked && Vector2.Distance(_inputSystem.MouseWorldPosition, _circleObject.transform.position) <= _circleObject.Radius;
    }
}
