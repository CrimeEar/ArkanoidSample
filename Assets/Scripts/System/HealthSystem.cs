using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private HealthUI _healthUI;

    private int _currentHealth;

    public int CurrentHealth{ get { return _currentHealth; } }
    public bool IsGameOver => CurrentHealth <= 0;

    public void Init()
    {
        _currentHealth = _startHealth;
        UpdateUI();
    }

    public void OnDeath()
    {
        _currentHealth -= 1;
        UpdateUI();
        if (IsGameOver)
        {
            GameStateMachine.Instance.StartNewState(0);
        }
    }

    private void UpdateUI()
    {
        _healthUI.OnUpdateHealthUI(CurrentHealth);
    }
}
