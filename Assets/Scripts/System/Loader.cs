using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private BordersObject _bordersObject;
    [SerializeField] private ParticlePoolSystem _particlesSystem;
    [SerializeField] private BackGroundMover _backGroundMover;
    [SerializeField] private SoundSystem _soundSystem;

    private void Awake()
    {
        _inputSystem.Init();

        InitStateContainer initContainer = new InitStateContainer(_inputSystem, _backGroundMover, _soundSystem, _bordersObject);
        _gameStateMachine.Init(initContainer);
    }
}
