using UnityEngine;

public class GameStatePlay : GameState
{
    [SerializeField] private int _looseBorderIndex = 3;
    [SerializeField] private BubblesHandler _bubblesHandler;
    [SerializeField] private PaddleObject _paddleObject;
    [SerializeField] private BallObject _ballObject;
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private HealthSystem _healthSystem;
    
    private InputSystem _inputSystem;
    private CollisionEventContainer _collisionContainer;
    private EndGameHandler _endGameHandler;
    private BordersObject _bordersObject;
    private BubblePopResultantContainer _bubblePopResultantContainer;

    private Vector3 _bubblesPopResultant;
    public Vector3 BubblePopResultant => _bubblesPopResultant;

    public override void Init(InitStateContainer initStateContainer)
    {
        _inputSystem = initStateContainer.InputSystem;
        _bordersObject = initStateContainer.BordersObject;
    }
    public override void StartState()
    {
        _uiObject.SetActive(true);
        _bubblePopResultantContainer = new BubblePopResultantContainer(Vector3.zero, 1f); // Temp
        _collisionContainer = new CollisionEventContainer();
        _endGameHandler = new EndGameHandler(_bordersObject, _looseBorderIndex, _collisionContainer, _healthSystem, _bubblePopResultantContainer, _bubblesHandler);

        _healthSystem.Init();
        _bubblesHandler.Init(_bubblePopResultantContainer, _ballObject);
        _ballObject.Init(_bubblesHandler.AllBaseCircles, _collisionContainer, _bubblePopResultantContainer, _healthSystem);
        _paddleObject.Init(_inputSystem, _bordersObject);
    }
    public override void EndState()
    {
        _uiObject.SetActive(false);
    }

    public override void UpdateState()
    {
        _inputSystem.CustomUpdate();

        if (_inputSystem.IsMouseClicked && !_ballObject.IsMove)
        {
            _ballObject.StartMove();
        }

        _paddleObject.CustomUpdate();
        _bubblesHandler.CustomUpdate();
        _ballObject.CustomUpdate();
        _endGameHandler.CustomUpdate();
    }
}
