using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameHandler
{
    private int _looseBorderIndex;

    private CollisionEventContainer _collisionEventContainer;
    private BordersObject _borders;
    private HealthSystem _healthSystem;

    public EndGameHandler(BordersObject bordersObject, int looseBorderIndex, CollisionEventContainer collisionEventContainer, HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _looseBorderIndex = looseBorderIndex;
        _borders = bordersObject;
        
        _collisionEventContainer = collisionEventContainer;
        _collisionEventContainer.OnBorderCollidedEvent += OnCollideBorder;
    }
    public void Release()
    {
        _collisionEventContainer.OnBorderCollidedEvent -= OnCollideBorder;
    }
    public void CustomUpdate()
    {

    }
    public void OnCollideBorder(Line borderLine)
    {
        if(borderLine == _borders.BorderLines[_looseBorderIndex])
        {
            _healthSystem.OnDeath();

            if(_healthSystem.CurrentHealth <= 0)
            {
                //loose screen
            }
            else
            {
                //stop state
                // reset
            }
        }
    }
}
