using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] private GameState[] _gameStates;

    public static GameStateMachine Instance;

    private int _currentStateIndex = 0;
    public void Init(InitStateContainer initStateContainer)
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        foreach (GameState state in _gameStates)
        {
            state.Init(initStateContainer);
        }
    }

    private void Start()
    {
        _currentStateIndex = 0;
        StartCurrentState();
    }
    private void Update()
    {
        UpdateCurrentState();
    }

    private void StartCurrentState()
    {
        _gameStates[_currentStateIndex].StartState();
    }
    private void UpdateCurrentState()
    {
        _gameStates[_currentStateIndex].UpdateState();
    }
    private void EndCurrentState()
    {
        _gameStates[_currentStateIndex].EndState();
    }
    public void StartNewState(int stateIndex)
    {
        EndCurrentState();
        _currentStateIndex = stateIndex;
        StartCurrentState();
    }
}
