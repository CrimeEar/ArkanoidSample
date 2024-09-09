using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    public abstract void Init(InitStateContainer initStateContainer);
    public abstract void StartState();
    public abstract void EndState();
    public abstract void UpdateState();
}

public class InitStateContainer
{
    public InputSystem InputSystem;
    public BackGroundMover BackGroundMover;
    public SoundSystem SoundSystem;
    public BordersObject BordersObject;

    public InitStateContainer(InputSystem inputSystem, BackGroundMover backGroundMover, SoundSystem soundSystem, BordersObject bordersObject)
    {
        InputSystem = inputSystem;
        BackGroundMover = backGroundMover;
        SoundSystem = soundSystem;
        BordersObject = bordersObject;
    }
}