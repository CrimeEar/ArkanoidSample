using UnityEngine;

public class MenuState : GameState
{
    [SerializeField] private BubblesHandler _menuBubbleHandler;
    [SerializeField] private CircleObject _startBubble;

    private StartGameHandler _startGameHandler;
    private InputSystem _inputSystem;
    public override void Init(InitStateContainer initStateContainer)
    {
        _inputSystem = initStateContainer.InputSystem;
        _startGameHandler = new StartGameHandler(_startBubble, _inputSystem);
    }
    public override void EndState()
    {
        _menuBubbleHandler.PopAllBubbles();
    }


    public override void StartState()
    {
        _menuBubbleHandler.Init();
    }

    public override void UpdateState()
    {
        _inputSystem.CustomUpdate();
        _menuBubbleHandler.CustomUpdate();
        _startGameHandler.CustomUpdate();
    }
}
