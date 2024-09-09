using UnityEngine;

public class GameStatePlay : GameState
{
    [SerializeField] private int _looseBorderIndex = 3;
    [SerializeField] private BubblesHandler _bubblesHandler;
    [SerializeField] private PaddleObject _paddleObject;
    [SerializeField] private BallObject _ballObject;
    
    private InputSystem _inputSystem;

    private CollisionEventContainer _collisionContainer;

    private EndGameHandler _endGameHandler;
    private HealthSystem _healthSystem;
    private BordersObject _bordersObject;

    public override void Init(InitStateContainer initStateContainer)
    {
        _inputSystem = initStateContainer.InputSystem;
        _bordersObject = initStateContainer.BordersObject;
    }
    public override void StartState()
    {
        _collisionContainer = new CollisionEventContainer();
        _endGameHandler = new EndGameHandler(_bordersObject, _looseBorderIndex, _collisionContainer, _healthSystem);

        _bubblesHandler.Init();
        _ballObject.Init(_bubblesHandler.AllCircles, _collisionContainer);
        _paddleObject.Init(_inputSystem, _bordersObject);
    }
    public override void EndState()
    {

    }

    public override void UpdateState()
    {
        _inputSystem.CustomUpdate();

        _paddleObject.CustomUpdate();
        _bubblesHandler.CustomUpdate();
        _ballObject.CustomUpdate();
    }

    
}
