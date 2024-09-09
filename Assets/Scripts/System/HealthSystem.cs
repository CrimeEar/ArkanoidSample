using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _startHealth;

    private int _currentHealth;

    public int CurrentHealth{ get { return _currentHealth; } }

    public void Init()
    {
        _currentHealth = _startHealth;
    }

    public void OnDeath()
    {
        _currentHealth -= 1;
    }
}
