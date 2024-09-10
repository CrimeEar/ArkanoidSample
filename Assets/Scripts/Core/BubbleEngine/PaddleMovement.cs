using DG.Tweening;
using UnityEngine;

public class PaddleMovement
{
    private const float MEMORY_TIME = 0.1f;
    private const float ON_COLLSIION_MOVE_VALUE = 0.2f;
    private const float ON_COLLISION_MOVE_DURATION = 0.1f;

    private PaddleObject _paddleObject;
    private BordersObject _bordersObject;
    private InputSystem _inputSystem;

    private Vector2 _velocity;
    private Vector2 _memoryVelocity;
    private Vector2 _currentPosition;

    private Tween _punchTween;

    private float _startYPosition;
    private float _currentMemoryTime;

    public Vector2 Position { get { return _currentPosition; } }
    public Vector2 Velocity { get { return _velocity; } }
    public Vector2 MemoryVelocity { get { return _memoryVelocity; } }
    public PaddleMovement(InputSystem inputSystem, PaddleObject paddleObject, BordersObject borders)
    {
        _inputSystem = inputSystem;
        _paddleObject = paddleObject;
        _currentPosition = (Vector2)paddleObject.transform.position;
        _velocity = Vector3.zero;
        _bordersObject = borders;
        _startYPosition = _paddleObject.transform.position.y;
    }
    public void Release()
    {
        TryKillTween();
    }
    public void CustomUpdate()
    {
        _velocity.x = _inputSystem.CurrentInputX; 

        if (_velocity.x != 0)
        {
            _memoryVelocity = _velocity;
            _currentMemoryTime = 0f;

            _currentPosition.x += _velocity.x * Time.deltaTime;
            _currentPosition.x = Mathf.Clamp(_currentPosition.x, _bordersObject.BorderLines[0].StartLinePoint.position.x + _paddleObject.Height / 2f, _bordersObject.BorderLines[2].StartLinePoint.position.x - _paddleObject.Height / 2f);
        }
        else
        {
            if(_memoryVelocity != Vector2.zero)
            {
                _currentMemoryTime += Time.deltaTime;
                if (_currentMemoryTime >= MEMORY_TIME)
                {
                    _memoryVelocity = Vector2.zero;
                }
            }
        }

        _paddleObject.transform.position = _currentPosition;
    }
    public void OnCollide()
    {
        TryKillTween();

        _punchTween = DOVirtual.Float(_startYPosition, _startYPosition - ON_COLLSIION_MOVE_VALUE, ON_COLLISION_MOVE_DURATION, OnPunchUpdate).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
    }
    private void OnPunchUpdate(float value)
    {
        _currentPosition.y = value;
    }
    private void TryKillTween()
    {
        if (_punchTween.IsActive())
        {
            _punchTween.Kill();
        }
    }
}
